using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.CustomTypes;
using UnityEngine;

namespace Assets.ScriptableObjects.Skills.PlayerSkills.MinigunSkill
{
    [CreateAssetMenu(fileName = "MinigunSkillSO", menuName = "Scriptable Objects/Skills/MinigunSkillSO")]
    public class MinigunSkillUpgradeableConfigSO : SkillUpgradeableStatsConfig
    {
        [Header("Turrets Stats")]
        [SerializeField] private float _turretRotationAngle = 120f;
        [SerializeField] private ByteUpgradeableStat _numberOfTurrets;
        [SerializeField] private FloatUpgradeableStat _turretRotationDuration;
        public TurretConfigSO TurretConfig { get; private set; }
        public FloatUpgradeableStat DelayBetweenSpawningBullets { get; private set; }
        public ByteUpgradeableStat NumberOfTurrets { get; private set; }
        public FloatUpgradeableStat TurretRotationDuration { get; private set; }

        [Header("Bullets Stats")]
        [SerializeField] private FloatUpgradeableStat _delayBetweenSpawningProjectile;
        [SerializeField] private FloatUpgradeableStat _startBulletDamage;
        [SerializeField] private FloatUpgradeableStat _startBulletSize;
        [SerializeField] private FloatUpgradeableStat _startBulletTimeToArriveAtRangeEnd;
        [SerializeField] private FloatUpgradeableStat _startShootingRange;
        [SerializeField] private ByteUpgradeableStat _startBulletMaxPiercing;
        public FloatUpgradeableStat BulletDamage { get; set; }
        public FloatUpgradeableStat BulletSize { get; set; }
        public FloatUpgradeableStat BulletTimeToArriveAtRangeEnd { get; set; }
        public FloatUpgradeableStat BulletRange { get; set; }
        public ByteUpgradeableStat BulletMaxPiercing { get; set; }

        private void OnEnable()
        {
            DelayBetweenSpawningBullets = _delayBetweenSpawningProjectile;
            NumberOfTurrets = _numberOfTurrets;
            TurretRotationDuration = _turretRotationDuration;

            BulletDamage = _startBulletDamage;
            BulletSize = _startBulletSize;
            BulletTimeToArriveAtRangeEnd = _startBulletTimeToArriveAtRangeEnd;
            BulletRange = _startShootingRange;
            BulletMaxPiercing = _startBulletMaxPiercing;

            ProjectileConfigSO projectileConfig = new ProjectileConfigSO()
            {
                Damage = BulletDamage.Value,
                Size = BulletSize.Value,
                TimeToArriveAtRangeEnd = BulletTimeToArriveAtRangeEnd.Value,
                Range = BulletRange.Value,
                MaxPiercing = BulletMaxPiercing.Value
            };

            TurretConfig = new TurretConfigSO
            (
                projectileConfig,
                _turretRotationAngle,
                TurretRotationDuration.Value
            );
        }
    }
}
