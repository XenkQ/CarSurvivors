using UnityEngine;

[CreateAssetMenu(fileName = "SawSkillSO", menuName = "Scriptable Objects/Skills/SawSkillSO")]
public class SawSkillConfigSO : ScriptableObject
{
    public float KnockbackPower;
    public float TimeToArriveAtKnockbackLocation;
    public float Damage;
    public float Size;
    public float AttackCooldown;
    public byte Level;
}
