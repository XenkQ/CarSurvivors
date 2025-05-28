using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.CustomTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigunSkillSO", menuName = "Scriptable Objects/Skills/MinigunSkillSO")]
public class MinigunSkillUpgradableConfigSO : SkillUpgradableConfig
{
    [field: SerializeField] public TurretConfigSO TurretStats { get; private set; }
    [SerializeField] private FloatUpgradableStat _delayBetweenSpawningProjectile;
    [SerializeField] private ByteUpgradableStat _numberOfTurrets;
    [SerializeField] private ByteUpgradableStat _level;

    public FloatUpgradableStat DelayBetweenSpawningProjectile { get; private set; }
    public ByteUpgradableStat NumberOfTurrets { get; private set; }
    public ByteUpgradableStat Level { get; private set; }

    private void OnEnable()
    {
        DelayBetweenSpawningProjectile = _delayBetweenSpawningProjectile;
        NumberOfTurrets = _numberOfTurrets;
        Level = _level;
    }

    protected override string[] ExcludedFieldNames { get; set; }
        = new string[] { nameof(TurretStats), nameof(NumberOfTurrets) };
}
