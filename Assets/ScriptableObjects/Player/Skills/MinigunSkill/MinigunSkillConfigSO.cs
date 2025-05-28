using Assets.ScriptableObjects.Player.Skills;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigunSkillSO", menuName = "Scriptable Objects/Skills/MinigunSkillSO")]
public class MinigunSkillConfigSO : SkillConfig
{
    public TurretConfigSO TurretStats;
    public float DelayBetweenSpawningProjectile;
    public byte NumberOfTurrets;
    public byte Level;

    protected override string[] ExcludedFieldNames { get; set; }
        = new string[] { nameof(TurretStats), nameof(NumberOfTurrets) };
}
