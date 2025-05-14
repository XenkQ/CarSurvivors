using System;
using UnityEngine;

namespace Assets.Scripts.Player.Skills.Minigun
{
    public class MinigunSkill : Skill
    {
        [SerializeField] private TurretStatsSO _minigunTurretsConfiguration;
        [SerializeField] private Projectile _turretsProejctile;
        [SerializeField] private Transform _projectilesParent;

        [SerializeField] private MinigunTurret[] _turrets;

        [SerializeField] private float _projectileSpawnDelay;
        private const float SPAWN_FIRST_PROJECTILE_DELAY = 0.5f;
        private ushort _level;

        public override event EventHandler OnLevelUp;

        private void Start()
        {
            InvokeRepeating("SpawnProjectile", SPAWN_FIRST_PROJECTILE_DELAY, _projectileSpawnDelay);
        }

        public override void LevelUp()
        {
            _level++;
            OnLevelUp?.Invoke(this, EventArgs.Empty);
        }

        private void SpawnProjectile()
        {
            foreach (MinigunTurret turret in _turrets)
            {
                Projectile projectile = Instantiate(_turretsProejctile, turret.GunTip.position, turret.GunTip.rotation, _projectilesParent);
                projectile.OnLifeEnd += Projectile_OnLifeEnd;
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
