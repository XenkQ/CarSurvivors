using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Scriptable Objects/EnemyStatsSO")]
public class EnemyStatsSO : ScriptableObject
{
    public float MovementSpeed;
    public float RotationSpeed;

    public float AnimationScaleMultiplier;

    public float Damage;

    public int Level;
    public float StunAfterDamageDuration;
}