using Assets.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Scriptable Objects/EnemyStatsSO")]
public class EnemyStatsSO : ScriptableObject
{
    public float MovementSpeed;
    public float RotationSpeed;
    public float Damage;
    public int Level;
    public float StunAfterDamageDuration;
    public float ExpFromKill;
    public GrowShrinkAnimationConfiguration GrowShrinkAnimationConfiguration;
}
