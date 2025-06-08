using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TurretStatsSO", menuName = "Scriptable Objects/TurretStatsSO")]
    public class TurretConfigSO : ScriptableObject
    {
        [field: SerializeField] public ProjectileConfigSO ProjectileStatsSO { get; set; }
        [field: SerializeField][Range(0, 180f)] public float RotationAngle { get; private set; }
        [field: SerializeField] public float RotationDuration { get; private set; }
    }
}
