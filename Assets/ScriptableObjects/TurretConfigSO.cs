using UnityEngine;

[CreateAssetMenu(fileName = "TurretStatsSO", menuName = "Scriptable Objects/TurretStatsSO")]
public class TurretConfigSO : ScriptableObject
{
    [field: SerializeField] public ProjectileConfigSO ProjectileStatsSO { get; private set; }
    [SerializeField][Range(0, 180f)] private float _startRotationAngle;
    [SerializeField] private float _startRotationDuration;

    public float RotationAngle { get; set; }
    public float RotationDuration { get; set; }

    private void OnEnable()
    {
        RotationAngle = _startRotationAngle;
        RotationDuration = _startRotationDuration;
    }
}
