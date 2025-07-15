using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.Stats;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.ScriptableObjects.Skills.PlayerSkills.MinigunSkill
{
    [CreateAssetMenu(fileName = "MinigunSkillSO", menuName = "Scriptable Objects/Skills/MinigunSkillSO")]
    public class MinigunSkillUpgradeableConfigSO : SkillUpgradeableStatsConfig
    {
        [Header("Turrets Stats")]
        [SerializeField] private TurretConfigSO _turretConfig;
        [SerializeField] private FloatUpgradeableStat _delayBetweenShootingBullets;
        [SerializeField] private FloatUpgradeableStat _range;
        [SerializeField] private ByteUpgradeableStat _numberOfTurrets;
        public TurretConfigSO TurretConfig => _turretConfig;
        public FloatUpgradeableStat DelayBetweenShoots { get; private set; }
        public FloatUpgradeableStat Range { get; private set; }
        public ByteUpgradeableStat NumberOfTurrets { get; private set; }

        [Header("Bullets Stats")]
        [SerializeField] private float _bulletTimeToArriveAtEndRangeMultiplier;
        [SerializeField] private ProjectileConfigSO _projectileConfig;
        [SerializeField] private FloatUpgradeableStat _startBulletSize;
        [SerializeField] private ByteUpgradeableStat _startBulletDamage;
        [SerializeField] private ByteUpgradeableStat _startBulletMaxPiercing;
        public FloatUpgradeableStat BulletSize { get; private set; }
        public ByteUpgradeableStat BulletDamage { get; private set; }
        public ByteUpgradeableStat BulletMaxPiercing { get; private set; }

        private void OnEnable()
        {
            DeepCopyUpgradeableStats();

            PrepareTurretConfig();

            PrepareProjectileConfig();

            TurretConfig.ProjectileStatsSO = _projectileConfig;
        }

        private void DeepCopyUpgradeableStats()
        {
            NumberOfTurrets = DeepCopyUtility.DeepCopy(_numberOfTurrets);
            Range = DeepCopyUtility.DeepCopy(_range);
            DelayBetweenShoots = DeepCopyUtility.DeepCopy(_delayBetweenShootingBullets);
            BulletDamage = DeepCopyUtility.DeepCopy(_startBulletDamage);
            BulletSize = DeepCopyUtility.DeepCopy(_startBulletSize);
            BulletMaxPiercing = DeepCopyUtility.DeepCopy(_startBulletMaxPiercing);
        }

        private void PrepareTurretConfig()
        {
            TurretConfig.Range = Range.Value;
            Range.OnUpgrade += (s, e) => TurretConfig.Range = Range.Value;
        }

        private void PrepareProjectileConfig()
        {
            _projectileConfig.Damage = BulletDamage.Value;
            _projectileConfig.Size = BulletSize.Value;
            _projectileConfig.Range = Range.Value;
            _projectileConfig.MaxPiercing = BulletMaxPiercing.Value;
            _projectileConfig.TimeToArriveAtEndRangeMultiplier = _bulletTimeToArriveAtEndRangeMultiplier;

            BulletDamage.OnUpgrade += (s, e) => _projectileConfig.Damage = BulletDamage.Value;
            BulletSize.OnUpgrade += (s, e) => _projectileConfig.Size = BulletSize.Value;
            Range.OnUpgrade += (s, e) => _projectileConfig.Range = Range.Value;
            BulletMaxPiercing.OnUpgrade += (s, e) => _projectileConfig.MaxPiercing = BulletMaxPiercing.Value;
        }
    }
}
