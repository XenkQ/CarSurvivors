using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts;
using Assets.Scripts.CustomTypes;
using UnityEngine;

namespace Assets.ScriptableObjects.Skills.PlayerSkills.MinigunSkill
{
    [CreateAssetMenu(fileName = "MinigunSkillSO", menuName = "Scriptable Objects/Skills/MinigunSkillSO")]
    public class MinigunSkillUpgradeableConfigSO : SkillUpgradeableStatsConfig
    {
        [Header("Turrets Stats")]
        [SerializeField] private TurretConfigSO _turretConfig;
        [SerializeField] private FloatUpgradeableStat _delayBetweenShootingBullets;
        [SerializeField] private ByteUpgradeableStat _numberOfTurrets;
        public TurretConfigSO TurretConfig { get; private set; }
        public FloatUpgradeableStat DelayBetweenSpawningBullets { get; private set; }
        public ByteUpgradeableStat NumberOfTurrets { get; private set; }

        [Header("Bullets Stats")]
        [SerializeField] private float _bulletTimeToArriveAtEndRangeMultiplier;
        [SerializeField] private ProjectileConfigSO _projectileConfig;
        [SerializeField] private FloatUpgradeableStat _startBulletDamage;
        [SerializeField] private FloatUpgradeableStat _startBulletSize;
        [SerializeField] private FloatUpgradeableStat _startShootingRange;
        [SerializeField] private ByteUpgradeableStat _startBulletMaxPiercing;
        public FloatUpgradeableStat BulletDamage { get; private set; }
        public FloatUpgradeableStat BulletSize { get; private set; }
        public FloatUpgradeableStat BulletRange { get; private set; }
        public ByteUpgradeableStat BulletMaxPiercing { get; private set; }

        private void OnEnable()
        {
            NumberOfTurrets = DeepCopyUtility.DeepCopy(_numberOfTurrets);

            TurretConfig = _turretConfig;

            DelayBetweenSpawningBullets = DeepCopyUtility.DeepCopy(_delayBetweenShootingBullets);
            BulletDamage = DeepCopyUtility.DeepCopy(_startBulletDamage);
            BulletSize = DeepCopyUtility.DeepCopy(_startBulletSize);
            BulletRange = DeepCopyUtility.DeepCopy(_startShootingRange);
            BulletMaxPiercing = DeepCopyUtility.DeepCopy(_startBulletMaxPiercing);

            _projectileConfig.Damage = BulletDamage.Value;
            _projectileConfig.Size = BulletSize.Value;
            _projectileConfig.Range = BulletRange.Value;
            _projectileConfig.MaxPiercing = BulletMaxPiercing.Value;
            _projectileConfig.TimeToArriveAtEndRangeMultiplier = _bulletTimeToArriveAtEndRangeMultiplier;

            BulletDamage.OnUpgrade += (s, e) => _projectileConfig.Damage = BulletDamage.Value;
            BulletSize.OnUpgrade += (s, e) => _projectileConfig.Size = BulletSize.Value;
            BulletRange.OnUpgrade += (s, e) => _projectileConfig.Range = BulletRange.Value;
            BulletMaxPiercing.OnUpgrade += (s, e) => _projectileConfig.MaxPiercing = BulletMaxPiercing.Value;

            TurretConfig.ProjectileStatsSO = _projectileConfig;
        }
    }
}
