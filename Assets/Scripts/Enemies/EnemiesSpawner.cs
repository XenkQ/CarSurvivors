using Assets.Scripts.CustomTypes;
using Assets.Scripts.GridSystem;
using Assets.Scripts.Spawners;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Enemies
{
    public sealed class EnemiesSpawner : MonoBehaviour, ISpawner
    {
        [Header("SpawnExpParticle Chance Settings")]
        [SerializeField] private FloatValueRange _spawnChanceDecreaseFactor;
        private EnemiesSpawnChanceRedistributionSystem _enemiesSpawnChanceRedistributionSystem = new();

        [Header("Enemies Pool settings")]
        [SerializeField] private Transform _enemiesParent;
        [SerializeField] private List<EnemySpawnInfo> _poolEnemiesInfo;
        private Dictionary<EnemySpawnInfo, ObjectPool<Enemy>> _enemyPools = new();

        public event EventHandler OnSpawnedEntityReleased;

        public ushort SpawnedEnemiesCounter { get; private set; }

        public static EnemiesSpawner Instance { get; private set; }

        private GridManager _gridManager;

        private EnemiesSpawner()
        {
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            PoolEnemies();
        }

        private void Start()
        {
            _gridManager = GridManager.Instance;

            EnemiesSpawnChanceRedistributionSystem.Configuration config = new()
            {
                EnemiesInfo = _poolEnemiesInfo,
                SpawnChanceDecreaseFactor = _spawnChanceDecreaseFactor
            };

            _enemiesSpawnChanceRedistributionSystem.Initialize(config);
        }

        private void PoolEnemies()
        {
            foreach (EnemySpawnInfo poolEnemyInfo in _poolEnemiesInfo)
            {
                ObjectPool<Enemy> currentEnemyPool = new(createFunc: () => Instantiate(poolEnemyInfo.EnemyPrefab, _enemiesParent),
                                                         actionOnGet: OnEnemyGet,
                                                         actionOnRelease: OnEnemyRelease,
                                                         defaultCapacity: poolEnemyInfo.MaxAmount,
                                                         maxSize: poolEnemyInfo.MaxAmount);

                _enemyPools.Add(poolEnemyInfo, currentEnemyPool);
            }
        }

        private void OnEnemyGet(Enemy enemy)
        {
            enemy.OnCanBeReleased += Enemy_OnRelease;

            Cell cell = GridCellsNotVisibleByMainCamera
                .GetRandomWalkableCells(_gridManager.GridPlayerChunk, 1)
                .FirstOrDefault();

            if (cell == null)
            {
                Debug.LogWarning("No walkable cells found for enemy spawn.");
                return;
            }

            enemy.transform.position = cell.WorldPos;

            enemy.OnGet();

            enemy.gameObject.SetActive(true);

            SpawnedEnemiesCounter++;
        }

        private void OnEnemyRelease(Enemy enemy)
        {
            enemy.OnCanBeReleased -= Enemy_OnRelease;

            enemy.OnRelease();

            enemy.gameObject.SetActive(false);

            OnSpawnedEntityReleased?.Invoke(enemy, EventArgs.Empty);

            SpawnedEnemiesCounter--;
        }

        private void Enemy_OnRelease(object sender, EventArgs e)
        {
            Enemy enemy = sender as Enemy;

            if (enemy is null)
            {
                return;
            }

            OnEnemyRelease(enemy);
        }

        public void SpawnRandomEnemiesBasedOnSpawnChance(int count)
        {
            for (int i = 0; i < count; i++)
            {
                EnemySpawnInfo currentEnemyToSpawnInfo = RandomEnemyInfoBasedOnSpawnChance();
                if (currentEnemyToSpawnInfo != null)
                {
                    _enemyPools[currentEnemyToSpawnInfo].Get();
                }
            }

            _enemiesSpawnChanceRedistributionSystem.RedistributeSpawnChance();
        }

        private EnemySpawnInfo RandomEnemyInfoBasedOnSpawnChance()
        {
            float totalChance = _poolEnemiesInfo.Sum(info => info.SpawnChanceInfo.SpawnChance);
            float randomPoint = UnityEngine.Random.value * totalChance;

            float currentSum = 0;
            foreach (EnemySpawnInfo enemySpawnInfo in _poolEnemiesInfo)
            {
                currentSum += enemySpawnInfo.SpawnChanceInfo.SpawnChance;
                if (currentSum >= randomPoint)
                {
                    return enemySpawnInfo;
                }
            }

            return null;
        }
    }
}
