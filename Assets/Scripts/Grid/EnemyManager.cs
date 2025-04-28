using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    [Serializable]
    private struct EnemyInfo
    {
        public Enemy enemyPrefab;
        public int maxAmount;
    }

    [Header("Pool info")]
    [SerializeField] private List<EnemyInfo> _poolEnemiesInfo;
    private Dictionary<EnemyInfo, ObjectPool<Enemy>> _enemyPools = new();

    private void Awake()
    {
        foreach (EnemyInfo poolEnemyInfo in _poolEnemiesInfo)
        {
            ObjectPool<Enemy> currentEnemyPool = new(createFunc: () => Instantiate(poolEnemyInfo.enemyPrefab, this.transform),
                                                     actionOnGet: SpawnEnemy,
                                                     actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
                                                     defaultCapacity: poolEnemyInfo.maxAmount,
                                                     maxSize: poolEnemyInfo.maxAmount);

            _enemyPools.Add(poolEnemyInfo, currentEnemyPool);
        }
    }

    private void Start()
    {
        for (int i = 0; i < _poolEnemiesInfo[0].maxAmount; i++)
        {
            Debug.Log(_enemyPools[_poolEnemiesInfo[0]].Get().gameObject.name);
        }
    }

    private void SpawnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }
}
