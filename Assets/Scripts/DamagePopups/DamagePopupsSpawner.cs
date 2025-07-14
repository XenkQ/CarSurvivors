using Assets.Scripts.CustomTypes;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.DamagePopups
{
    public interface IDamagePopupsSpawner
    {
        void SpawnDamagePopup(Vector3 center, float damage, SpawnShapeModes spawnShapeMode);
    }

    public sealed class DamagePopupsSpawner : MonoBehaviour, IDamagePopupsSpawner
    {
        [Serializable]
        private struct VisualApearanceByDamageTreshold
        {
            [SerializeField] public float Treshold;
            [SerializeField] public FloatValueRange FontSizesRanges;
            [SerializeField] public Color Color;
        }

        public static DamagePopupsSpawner Instance;

        [SerializeField] private float _damagePopupVisibilityDuration;
        [SerializeField] private DamagePopup _damagePopupPrefab;
        [SerializeField] private VisualApearanceByDamageTreshold[] visualApearanceByDamageTresholds;
        [SerializeField] private FloatValueRange _popupsSpeedRange;
        [SerializeField] private float _popupsMovementRange = 1f;

        private DamagePopupsSpawner()
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

        public void SpawnDamagePopup(Vector3 center, float damage, SpawnShapeModes spawnShapeMode)
        {
            if (visualApearanceByDamageTresholds.Length == 0)
            {
                Debug.Log("NOT SPAWNING POPUP");
                Debug.LogError("There is 0 colors by damage tresholds entries in: " + transform.name);
                return;
            }

            DamagePopup damagePopup = Instantiate(_damagePopupPrefab, center, Quaternion.identity);

            VisualApearanceByDamageTreshold? visualApearanceByDamageTreshold
                = FindCorrectVisualApearanceByTreshold(damage);

            if (visualApearanceByDamageTreshold is null)
            {
                return;
            }

            float fontSize = CalculateCorrectFontSize(visualApearanceByDamageTreshold.Value, damage);
            damagePopup.Initialize(new DamagePopupConfig(damage, fontSize, visualApearanceByDamageTreshold.Value.Color));

            Vector3 dest = GetDestinationBasedOnSpawnShapeMode(center, spawnShapeMode);

            damagePopup
                .transform
                .DOMove(dest, _damagePopupVisibilityDuration)
                .SetEase(Ease.InOutSine);

            Destroy(damagePopup.gameObject, _damagePopupVisibilityDuration);
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

        private float CalculateCorrectFontSize(VisualApearanceByDamageTreshold visualApearanceByDamageTreshold, float damage)
        {
            float minSize = visualApearanceByDamageTreshold.FontSizesRanges.Min;
            float maxSize = visualApearanceByDamageTreshold.FontSizesRanges.Max;
            float calculatedSize = damage / maxSize;

            if (calculatedSize < minSize)
            {
                calculatedSize = minSize;
            }

            if (calculatedSize > maxSize)
            {
                calculatedSize = maxSize;
            }

            return calculatedSize;
        }
    }
}
