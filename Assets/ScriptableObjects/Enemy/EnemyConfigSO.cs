using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Scriptable Objects/EnemyStatsSO")]
public class EnemyConfigSO : ScriptableObject
{
    public float MovementSpeed;
    public float RotationSpeed;

    public float AnimationScaleMultiplier;

    public float Damage;

    public byte Level;
    public float StunAfterDamageDuration;
}
