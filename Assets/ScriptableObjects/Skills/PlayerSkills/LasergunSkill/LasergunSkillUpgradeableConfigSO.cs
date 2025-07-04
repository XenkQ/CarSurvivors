using Assets.ScriptableObjects;
using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.Stats;
using Assets.Scripts.Utils;
using UnityEngine;

[CreateAssetMenu(fileName = "LasergunSkillSO", menuName = "Scriptable Objects/Skills/LasergunSkillSO")]
public class LasergunSkillSO : SkillUpgradeableStatsConfig
{
    [Header("Turrets Stats")]
    public float SearchForTargetInterval { get; private set; } = 0.2f;

    [SerializeField] private TurretConfigSO _turretConfig;
    [SerializeField] private FloatUpgradeableStat _delayBetweenShoots;
    [SerializeField] private ByteUpgradeableStat _numberOfTurrets;
    public TurretConfigSO TurretConfig => _turretConfig;
    public FloatUpgradeableStat DelayBetweenShoots { get; private set; }
    public ByteUpgradeableStat NumberOfTurrets { get; private set; }

    [Header("Laser Stats")]
    [SerializeField] private ProjectileConfigSO _projectileConfig;
    [SerializeField] private FloatUpgradeableStat _startDamage;
    [SerializeField] private FloatUpgradeableStat _startRange;
    public FloatUpgradeableStat Damage { get; private set; }
    public FloatUpgradeableStat Range { get; private set; }

    private void OnEnable()
    {
        DeepCopyUpgradeableStats();

        PrepareProjectileConfig();

        TurretConfig.ProjectileStatsSO = _projectileConfig;
    }

    private void DeepCopyUpgradeableStats()
    {
        NumberOfTurrets = DeepCopyUtility.DeepCopy(_numberOfTurrets);
        DelayBetweenShoots = DeepCopyUtility.DeepCopy(_delayBetweenShoots);
        Damage = DeepCopyUtility.DeepCopy(_startDamage);
        Range = DeepCopyUtility.DeepCopy(_startRange);
    }

    private void PrepareProjectileConfig()
    {
        _projectileConfig.Range = Range.Value;
        _projectileConfig.Damage = Damage.Value;

        Range.OnUpgrade += (s, e) => _projectileConfig.Range = Range.Value;
        Damage.OnUpgrade += (s, e) => _projectileConfig.Damage = Damage.Value;
    }
}
