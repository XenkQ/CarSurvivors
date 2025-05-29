using Assets.Scripts.Enemies;
using Assets.Scripts.GridSystem;
using Assets.Scripts.HealthSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts
{
    public sealed class EnemiesSpawner : MonoBehaviour
    {
        [Serializable]
        private class EnemyInfo
        {
            public Enemy enemyPrefab;
            public ushort maxAmount;
            public float spawnChance;
        }

        [SerializeField] private List<EnemyInfo> _poolEnemiesInfo;
        private Dictionary<EnemyInfo, ObjectPool<Enemy>> _enemyPools = new();
        public ushort SpawnedEnemiesCounter { get; private set; }

        public static EnemiesSpawner Instance { get; private set; }

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

        private void PoolEnemies()
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

        private void OnEnemyGet(Enemy enemy)
        {
            enemy.transform.position = RandomGridCellWithConditionFinder
                .GetRandomWalkableEdgeCellOrNull(GridManager.Instance.GridPlayerChunk)
                .WorldPos;
            enemy.Health.OnNoHealth += Health_OnNoHealth;
            enemy.gameObject.SetActive(true);
            SpawnedEnemiesCounter++;
        }

        private void OnEnemyRelease(Enemy enemy)
        {
            enemy.Health.OnNoHealth -= Health_OnNoHealth;
            enemy.gameObject.SetActive(false);
            SpawnedEnemiesCounter--;
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

        public void SpawnRandomEnemiesBasedOnSpawnChance(int count)
        {
            for (int i = 0; i < count; i++)
            {
                EnemyInfo currentEnemyToSpawnInfo = RandomEnemyInfoBasedOnSpawnChance();
                if (currentEnemyToSpawnInfo != null)
                {
                    _enemyPools[currentEnemyToSpawnInfo].Get();
                }
            }
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
    }
}
