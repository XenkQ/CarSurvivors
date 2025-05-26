using Assets.ScriptableObjects.Player.Skills;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigunSkillSO", menuName = "Scriptable Objects/Skills/MinigunSkillSO")]
public class MinigunSkillConfigSO : SkillConfig
{
    public byte NumberOfTurrets;
    public float DelayBetweenSpawningProjectile;
    public TurretConfigSO TurretStats;
    public byte Level;

    protected override string[] ExcludedFieldNames { get; set; }
        = new string[] { nameof(TurretStats), nameof(NumberOfTurrets) };
}
