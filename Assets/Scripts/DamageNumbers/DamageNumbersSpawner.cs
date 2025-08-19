using Assets.Scripts.CustomTypes;
using Assets.Scripts.ObjectLifeCycle.Actions;
using Assets.Scripts.Shapes;
using Assets.Scripts.Spawners.WorldSpace;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.DamageNumbers
{
    public class DamageNubmersSpawnerConfig
    {
        public float Damage;
        public ShapeModes SpawnShapeMode;

        public DamageNubmersSpawnerConfig(float damage, ShapeModes spawnShapeMode)
        {
            Damage = damage;
            SpawnShapeMode = spawnShapeMode;
        }

        public void Deconstruct(out float damage, out ShapeModes spawnShapeMode)
        {
            damage = Damage;
            spawnShapeMode = SpawnShapeMode;
        }
    }

    public class DamageNumbersSpawner : MonoBehaviour,
        IInWorldSpaceSpawner<DamageNumbersSpawner, DamageNubmersSpawnerConfig>,
        IEnableDisableFunctionalityTrigger<DamageNumbersSpawner>
    {
        [Serializable]
        private struct VisualApearanceByDamageTreshold
        {
            [SerializeField] public float Treshold;
            [SerializeField] public DamageNumberApearance DamagePopupApearance;

            public VisualApearanceByDamageTreshold(float treshold, DamageNumberApearance damagePopupApearance)
            {
                Treshold = treshold;
                DamagePopupApearance = damagePopupApearance;
            }
        }

        [SerializeField] private float _damagePopupVisibilityDuration;
        [SerializeField] private DamageNumber _damagePopupPrefab;
        [SerializeField] private VisualApearanceByDamageTreshold[] visualApearanceByDamageTresholds;
        [SerializeField] private FloatValueRange _popupsSpeedRange;
        [SerializeField] private float _popupsMovementRange = 1f;
        private bool _isPopupsEnabled = true;

        public event EventHandler OnSpawnedEntityReleased;

        public uint CurrentlySpawnedObjectsCount { get; private set; }

        private IObjectPool<DamageNumber> _damageNumberPool;

        private void Awake()
        {
            _damageNumberPool = new ObjectPool<DamageNumber>(
                createFunc: () => Instantiate(_damagePopupPrefab, transform),
                actionOnGet: (obj) => obj.gameObject.SetActive(true),
                actionOnRelease: (obj) => obj.gameObject.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj.gameObject),
                collectionCheck: false,
                defaultCapacity: 20,
                maxSize: 100
            );
        }

        public void EnableFunctionality()
        {
            _isPopupsEnabled = true;
        }

        public void DisableFunctionality()
        {
            _isPopupsEnabled = false;
        }

        public void Spawn(Vector3 pos, DamageNubmersSpawnerConfig specificConfig, int count = 1)
        {
            if (!_isPopupsEnabled)
            {
                return;
            }

            if (visualApearanceByDamageTresholds.Length == 0)
            {
                Debug.Log("NOT SPAWNING POPUP");
                Debug.LogError("There is 0 colors by damage tresholds entries in: " + transform.name);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                DamageNumber damageNumber = _damageNumberPool.Get();
                damageNumber.transform.position = pos;
                damageNumber.transform.rotation = Quaternion.identity;

                var (damage, spawnShapeMode) = specificConfig;

                VisualApearanceByDamageTreshold? visualApearanceByDamageTreshold
                    = FindCorrectVisualApearanceByTreshold(damage);

                if (visualApearanceByDamageTreshold is null)
                {
                    _damageNumberPool.Release(damageNumber);
                    return;
                }

                damageNumber.Initialize(new DamageNumberConfig(damage, visualApearanceByDamageTreshold.Value.DamagePopupApearance));

                damageNumber.OnLifeEnd += DamageNumber_OnLifeEnd;

                Vector3 dest = GetDestinationBasedOnSpawnShapeMode(pos, spawnShapeMode);
                damageNumber
                    .transform
                    .DOMove(dest, _damagePopupVisibilityDuration)
                    .SetEase(Ease.InOutSine);

                CurrentlySpawnedObjectsCount++;
            }
        }

        private void DamageNumber_OnLifeEnd(object sender, EventArgs args)
        {
            if (sender is DamageNumber damageNumber)
            {
                CurrentlySpawnedObjectsCount--;
                damageNumber.OnLifeEnd -= DamageNumber_OnLifeEnd;
                _damageNumberPool.Release(damageNumber);
                OnSpawnedEntityReleased?.Invoke(damageNumber, EventArgs.Empty);
            }
        }

        private Vector3 GetDestinationBasedOnSpawnShapeMode(Vector3 startPos, ShapeModes spawnShapeMode)
        {
            return spawnShapeMode switch
            {
                ShapeModes.Sphere => RandomUtility.GetRandomPointOnSphereSurface(startPos, _popupsMovementRange),
                ShapeModes.Hemisphere => RandomUtility.GetRandomPointOnHemisphereSurface(startPos, _popupsMovementRange),
                _ => transform.position,
            };
        }

        private VisualApearanceByDamageTreshold? FindCorrectVisualApearanceByTreshold(float damage)
        {
            for (int i = visualApearanceByDamageTresholds.Length - 1; i >= 0; i--)
            {
                if (visualApearanceByDamageTresholds[i].Treshold <= damage)
                {
                    return visualApearanceByDamageTresholds[i];
                }
            }

            return null;
        }
    }
}
