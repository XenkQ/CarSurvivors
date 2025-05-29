using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.CustomTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigunSkillSO", menuName = "Scriptable Objects/Skills/MinigunSkillSO")]
public class MinigunSkillUpgradeableConfigSO : SkillUpgradeableConfig
{
    [field: SerializeField] public TurretConfigSO TurretStats { get; private set; }
    [SerializeField] private FloatUpgradeableStat _delayBetweenSpawningProjectile;
    [SerializeField] private ByteUpgradeableStat _numberOfTurrets;

    public FloatUpgradeableStat DelayBetweenSpawningProjectile { get; private set; }
    public ByteUpgradeableStat NumberOfTurrets { get; private set; }

    private void OnEnable()
    {
        DelayBetweenSpawningProjectile = _delayBetweenSpawningProjectile;
        NumberOfTurrets = _numberOfTurrets;
    }

    protected override string[] ExcludedFieldNames { get; set; }
        = new string[] { nameof(TurretStats), nameof(NumberOfTurrets) };
}
