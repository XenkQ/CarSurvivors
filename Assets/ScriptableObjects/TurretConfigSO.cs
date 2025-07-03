using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TurretStatsSO", menuName = "Scriptable Objects/TurretStatsSO")]
    public class TurretConfigSO : ScriptableObject
    {
        [SerializeField] private float _startRange;

        [field: SerializeField] public ProjectileConfigSO ProjectileStatsSO { get; set; }
        [field: SerializeField, Range(0, 360f)] public float RotationAngle { get; private set; }
        [field: SerializeField] public float RotationDuration { get; private set; }
        [field: SerializeField] public float SearchForTargetInterval { get; private set; }
        public float Range { get; set; }

        private void OnEnable()
        {
            Range = _startRange;
        }
    }
}
