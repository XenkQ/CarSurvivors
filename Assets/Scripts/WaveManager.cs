using Assets.Scripts;
using Assets.Scripts.GridSystem;
using Assets.Scripts.HealthSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts
{
    public sealed class WaveManager : MonoBehaviour
    {
        [Serializable]
        private class EnemyInfo
        {
            public Enemy enemyPrefab;
            public ushort maxAmount;
            public float spawnChance;
        }

        [SerializeField] private float _startSpawnWaveDelay = 8f;
        private float _currentSpawnWaveDelay;
        private ushort _spawnedEnemiesCounter;
        private ushort _waveMaxEnemies = 4;
        private float _firstWaveDelay = 1f;
        private int _wave = 1;

        private float _startEnemiesOutsidePlayerChunkCheckDelay = 2f;
        private float _currentEnemiesOutsidePlayerChunkCheckDelay;

        [SerializeField] private List<EnemyInfo> _poolEnemiesInfo;
        private Dictionary<EnemyInfo, ObjectPool<Enemy>> _enemyPools = new();

        private WaveManager()
        { }

        private void Awake()
        {
            foreach (EnemyInfo poolEnemyInfo in _poolEnemiesInfo)
            {
                ObjectPool<Enemy> currentEnemyPool = new(createFunc: () => Instantiate(poolEnemyInfo.enemyPrefab, this.transform),
                                                         actionOnGet: OnEnemyGet,
                                                         actionOnRelease: OnEnemyRelease,
                                                         defaultCapacity: poolEnemyInfo.maxAmount,
                                                         maxSize: poolEnemyInfo.maxAmount);

                _enemyPools.Add(poolEnemyInfo, currentEnemyPool);
            }
        }

        private void Start()
        {
            _currentSpawnWaveDelay = _firstWaveDelay;
        }

        private void Update()
        {
            WavesProcess();
            EnemiesToPlayerChunkTeleporter();
        }

        private void WavesProcess()
        {
            if ((_wave == 1 || _spawnedEnemiesCounter > 0) && _currentSpawnWaveDelay > 0)
            {
                _currentSpawnWaveDelay -= Time.deltaTime;
            }
            else
            {
                SpawnWave();
                _currentSpawnWaveDelay = _startSpawnWaveDelay;
            }
        }

        private void SpawnWave()
        {
            for (int i = 0; i < _waveMaxEnemies; i++)
            {
                EnemyInfo currentEnemyToSpawnInfo = RandomEnemyInfoBasedOnSpawnChance();
                if (currentEnemyToSpawnInfo != null)
                {
                    _enemyPools[currentEnemyToSpawnInfo].Get();
                }
            }
            _wave++;
        }

        private EnemyInfo RandomEnemyInfoBasedOnSpawnChance()
        {
            float totalChance = _poolEnemiesInfo.Sum(info => info.spawnChance);
            float randomPoint = UnityEngine.Random.value * totalChance;

            float currentSum = 0;
            foreach (EnemyInfo enemyInfo in _poolEnemiesInfo)
            {
                currentSum += enemyInfo.spawnChance;
                if (currentSum >= randomPoint)
                {
                    return enemyInfo;
                }
            }

            return null;
        }

        private void EnemiesToPlayerChunkTeleporter()
        {
            if (_currentEnemiesOutsidePlayerChunkCheckDelay > 0)
            {
                _currentEnemiesOutsidePlayerChunkCheckDelay -= Time.deltaTime;
            }
            else
            {
                _currentEnemiesOutsidePlayerChunkCheckDelay = _startEnemiesOutsidePlayerChunkCheckDelay;
                foreach (Enemy enemy in GetEnemiesOutsidePlayerChunk())
                {
                    enemy.transform.position = GetSpawnPos();
                }
            }
        }

        private Vector3 GetSpawnPos()
        {
            return GridManager.Instance.GridPlayerChunk.GetRandomWalkableEdgeCell().WorldPos;
        }

        private Enemy[] GetEnemiesOutsidePlayerChunk()
        {
            List<Enemy> enemies = new List<Enemy>();

            Assets.Scripts.GridSystem.Grid playerChunk = GridManager.Instance.GridPlayerChunk;
            Cell centerCell = playerChunk.Cells[playerChunk.Width / 2, playerChunk.Height / 2];
            Vector3 center = centerCell.WorldPos;

            float width = playerChunk.Width * 0.5f * playerChunk.CellSize;
            float height = playerChunk.Height * 0.5f * playerChunk.CellSize;

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out Enemy enemy))
                {
                    Vector3 enemyPos = enemy.transform.position;
                    if (Mathf.Abs(enemyPos.x - center.x) > width || Mathf.Abs(enemyPos.z - center.z) > height)
                    {
                        enemies.Add(enemy);
                    }
                }
            }

            return enemies.ToArray();
        }

        private void OnEnemyGet(Enemy enemy)
        {
            enemy.transform.position = GetSpawnPos();
            enemy.Health.OnNoHealth += Health_OnNoHealth;
            enemy.gameObject.SetActive(true);
            _spawnedEnemiesCounter++;
        }

        private void OnEnemyRelease(Enemy enemy)
        {
            enemy.Health.OnNoHealth -= Health_OnNoHealth;
            enemy.gameObject.SetActive(false);
            _spawnedEnemiesCounter--;
        }

        private void Health_OnNoHealth(object sender, EventArgs e)
        {
            Health healthComponent = sender as Health;
            if (healthComponent == null)
            {
                return;
            }

            if (healthComponent.gameObject.TryGetComponent(out Enemy enemy))
            {
                OnEnemyRelease(enemy);
            }
        }
    }
}
