using Assets.Scripts.Player.Skills;
using System;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.Minigun
{
    public class MinigunSkill : Skill<MinigunSkillConfigSO>
    {
        [field: SerializeField] public override StartEndScriptableConfig<MinigunSkillConfigSO> StartEndScriptableConfig { get; protected set; }
        public override MinigunSkillConfigSO CurrentConfig { get; set; }

        [SerializeField] private Projectile _turretsProejctile;
        [SerializeField] private Transform _projectilesParent;
        [SerializeField] private MinigunTurret[] _turrets;

        public override void Initialize()
        {
            base.Initialize();
            InvokeRepeating("SpawnProjectile",
                            CurrentConfig.DelayBetweenSpawningProjectile,
                            CurrentConfig.DelayBetweenSpawningProjectile);

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
                turret.Initialize(CurrentConfig.TurretStats);
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
