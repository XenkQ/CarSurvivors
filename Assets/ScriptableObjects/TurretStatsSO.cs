using UnityEngine;

[CreateAssetMenu(fileName = "TurretStatsSO", menuName = "Scriptable Objects/TurretStatsSO")]
public class TurretStatsSO : ScriptableObject
{
    [field: SerializeField] public ProjectileStatsSO ProjectileStatsSO { get; private set; }
    [SerializeField] private float _startRadiusAngle;
    [SerializeField] private float _startRotationDuration;
    [Range(0, 180f)] public float RadiusAngle { get; set; }
    public float RotationDuration { get; set; }

    private void OnEnable()
    {
        RadiusAngle = _startRadiusAngle;
        RotationDuration = _startRotationDuration;
    }
}
