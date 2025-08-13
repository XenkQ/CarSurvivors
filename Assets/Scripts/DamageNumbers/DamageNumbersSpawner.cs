using Assets.Scripts.CustomTypes;
using Assets.Scripts.DamagePopups;
using Assets.Scripts.Spawners.WorldSpace;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.DamageNumbers
{
    public class DamageNubmersSpawnerConfig
    {
        public float Damage;
        public SpawnShapeModes SpawnShapeMode;

        public DamageNubmersSpawnerConfig(float damage, SpawnShapeModes spawnShapeMode)
        {
            Damage = damage;
            SpawnShapeMode = spawnShapeMode;
        }

        public void Deconstruct(out float damage, out SpawnShapeModes spawnShapeMode)
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

        public uint CurrentlySpawnedObjectsCount { get; private set; }

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
                DamageNumber damageNumber = Instantiate(_damagePopupPrefab, pos, Quaternion.identity);
                var (damage, spawnShapeMode) = specificConfig;

                VisualApearanceByDamageTreshold? visualApearanceByDamageTreshold
                    = FindCorrectVisualApearanceByTreshold(damage);

                if (visualApearanceByDamageTreshold is null)
                {
                    return;
                }

                damageNumber.Initialize(new DamageNumberConfig(damage, visualApearanceByDamageTreshold.Value.DamagePopupApearance));

                damageNumber.OnLifeEnd += (sender, args) =>
                {
                    CurrentlySpawnedObjectsCount--;
                    Destroy(damageNumber.gameObject);
                };

                Vector3 dest = GetDestinationBasedOnSpawnShapeMode(pos, spawnShapeMode);
                damageNumber
                    .transform
                    .DOMove(dest, _damagePopupVisibilityDuration)
                    .SetEase(Ease.InOutSine);

                CurrentlySpawnedObjectsCount++;
            }
        }

        private Vector3 GetDestinationBasedOnSpawnShapeMode(Vector3 startPos, SpawnShapeModes spawnShapeMode)
        {
            return spawnShapeMode switch
            {
                SpawnShapeModes.Sphere => RandomUtility.GetRandomPointOnSphereSurface(startPos, _popupsMovementRange),
                SpawnShapeModes.Hemisphere => RandomUtility.GetRandomPointOnHemisphereSurface(startPos, _popupsMovementRange),
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
