using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Scriptable Objects/EnemyStatsSO")]
public class EnemyConfigSO : ScriptableObject
{
    public float MovementSpeed;
    public float RotationSpeed;

    public float Damage;

    public byte DangerLevel;

    public float ExpForKill;

    public bool IsMovingByCrawling;
}
