using Assets.ScriptableObjects.Skills;
using Assets.ScriptableObjects.Skills.PlayerSkills.MinigunSkill;
using System;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.Minigun
{
    public class MinigunSkill : UpgradeableSkill<MinigunSkillUpgradeableConfigSO>
    {
        [field: SerializeField] public override SkillInfoSO SkillInfo { get; protected set; }
        [field: SerializeField] protected override MinigunSkillUpgradeableConfigSO _config { get; set; }
        [SerializeField] private Projectile _turretsProejctile;
        [SerializeField] private Transform _projectilesParent;
        [SerializeField] private MinigunTurret[] _turrets;

        public override void Initialize()
        {
            base.Initialize();

            InvokeRepeating(nameof(SpawnProjectile),
                            _config.DelayBetweenSpawningBullets.Value,
                            _config.DelayBetweenSpawningBullets.Value);

            InitializeTurrets();
        }

        private void SpawnProjectile()
        {
            foreach (MinigunTurret turret in _turrets)
            {
                Projectile projectile = Instantiate(_turretsProejctile, turret.GunTip.position, turret.GunTip.rotation, _projectilesParent);
                projectile.OnLifeEnd += Projectile_OnLifeEnd;
            }
        }

        private void InitializeTurrets()
        {
            foreach (MinigunTurret turret in _turrets)
            {
                turret.Initialize(_config.TurretConfig);
            }
        }

        private void Projectile_OnLifeEnd(object sender, EventArgs e)
        {
            Projectile projectile = sender as Projectile;
            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }
    }
}
