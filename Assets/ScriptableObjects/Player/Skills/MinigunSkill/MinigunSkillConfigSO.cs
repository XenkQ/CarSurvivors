using UnityEngine;

[CreateAssetMenu(fileName = "MinigunSkillSO", menuName = "Scriptable Objects/Skills/MinigunSkillSO")]
public class MinigunSkillConfigSO : ScriptableObject
{
    public byte NumberOfTurrets;
    public float DelayBetweenSpawningProjectile;
    public TurretConfigSO TurretStats;
    public byte Level;
}
