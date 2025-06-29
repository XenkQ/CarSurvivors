using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GridSystem;
using UnityEngine;

namespace Assets.Scripts.Skills.ObjectsImpactingSkills.Crate
{
    public sealed class CollectibleItemsSpawner : MonoBehaviour, ISpawner
    {
        [Serializable]
        private struct CollectibleItemSpawnData
        {
            public GameObject Prefab;
            public float SpawnYOffset;
            public float SpawnChance;
        }

        private const byte MAX_SPAWN_COUNT = 6;
        [SerializeField] private Transform _collectibleItemsParent;
        [SerializeField] private float _spawnDelay = 8f;
        [SerializeField] private CollectibleItemSpawnData[] _collectibleItemsSpawnData;
        private List<ICollectible> _spawnedCollectibleItems = new List<ICollectible>(MAX_SPAWN_COUNT);

        public event EventHandler OnSpawnedEntityReleased;

        public static CollectibleItemsSpawner Instance { get; private set; }

        private CollectibleItemsSpawner()
        { }

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
        }

        private void Start()
        {
            InvokeRepeating(nameof(SpawnCollectibleItemAtRandomNotOccupiedCell), _spawnDelay, _spawnDelay);
        }

        private void SpawnCollectibleItemAtRandomNotOccupiedCell()
        {
            if (_spawnedCollectibleItems.Count < MAX_SPAWN_COUNT)
            {
                Cell drawnCell = RandomWalkableCellsFinder
                    .FindCellWithoutCollectible(GridManager.Instance.WorldGrid);

                if (drawnCell == null)
                {
                    Debug.LogError("No walkable cell without collectible found for spawning a skill crate.");
                    return;
                }

                CollectibleItemSpawnData? collectibleItemSpawnData = RandomCollectibleItemBasedOnSpawnChance();
                if (collectibleItemSpawnData != null)
                {
                    GameObject collectibleObject = Instantiate(
                        collectibleItemSpawnData.Value.Prefab,
                        new Vector3(drawnCell.WorldPos.x, collectibleItemSpawnData.Value.SpawnYOffset, drawnCell.WorldPos.z),
                        Quaternion.identity,
                        _collectibleItemsParent
                    );

                    if (collectibleObject.TryGetComponent(out ICollectible collectible))
                    {
                        collectible.OnCollected += Collectible_OnCollected;

                        _spawnedCollectibleItems.Add(collectible);

                        drawnCell.IsOccupiedByCollectible = true;
                    }
                }
            }
        }

        private CollectibleItemSpawnData? RandomCollectibleItemBasedOnSpawnChance()
        {
            float totalChance = _collectibleItemsSpawnData.Sum(info => info.SpawnChance);
            float randomPoint = UnityEngine.Random.value * totalChance;

            float currentSum = 0;
            foreach (CollectibleItemSpawnData collectibleItem in _collectibleItemsSpawnData)
            {
                currentSum += collectibleItem.SpawnChance;
                if (currentSum >= randomPoint)
                {
                    return collectibleItem;
                }
            }

            return null;
        }

        private void Collectible_OnCollected(object sender, EventArgs e)
        {
            var collectibleGameObject = (sender as ICollectible)?.GameObject;

            if (collectibleGameObject != null && collectibleGameObject.TryGetComponent(out ICollectible collectible))
            {
                ReleaseOccupiedCellByCollectible(collectibleGameObject);

                _spawnedCollectibleItems.Remove(collectible);

                OnSpawnedEntityReleased?.Invoke(collectible, EventArgs.Empty);
            }
        }

        private void ReleaseOccupiedCellByCollectible(GameObject collectable)
        {
            Cell occupiedCell = WorldPosToCellConverter
                 .GetCellFromGridByWorldPos(GridManager.Instance.WorldGrid, collectable.transform.position);

            if (occupiedCell != null)
            {
                occupiedCell.IsOccupiedByCollectible = false;
            }
        }
    }
}
