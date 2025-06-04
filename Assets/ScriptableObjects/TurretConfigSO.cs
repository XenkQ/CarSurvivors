using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TurretStatsSO", menuName = "Scriptable Objects/TurretStatsSO")]
    public class TurretConfigSO : ScriptableObject
    {
        [field: SerializeField] public ProjectileConfigSO ProjectileStatsSO { get; private set; }
        [SerializeField][Range(0, 180f)] private float _startRotationAngle;
        [SerializeField] private float _startRotationDuration;
        public float RotationAngle { get; set; }
        public float RotationDuration { get; set; }

        public TurretConfigSO(ProjectileConfigSO projectileStatsSO, float startRotationAngle, float startRotationDuration)
        {
            ProjectileStatsSO = projectileStatsSO;
            _startRotationAngle = startRotationAngle;
            _startRotationDuration = startRotationDuration;
        }

        private void OnEnable()
        {
            RotationAngle = _startRotationAngle;
            RotationDuration = _startRotationDuration;
        }
    }
}
