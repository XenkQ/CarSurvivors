using System;
using UnityEngine;

namespace Player.Skills
{
    public class MinigunSkill : Skill
    {
        [SerializeField] private Projectile _turretsProejctile;
        [SerializeField] private MinigunTurret[] _turrets;
        [SerializeField] private float _projectileSpawnDelay;
        private const float SPAWN_FIRST_PROJECTILE_DELAY = 0.5f;
        private ushort _level;

        public override event EventHandler OnLevelUp;

        private void OnEnable()
        {
            InvokeRepeating("SpawnProjectile", SPAWN_FIRST_PROJECTILE_DELAY, _projectileSpawnDelay)];
        }

        private void SpawnProjectile()
        {
            foreach (MinigunTurret turret in _turrets)
            {
                Projectile projectile = Instantiate(_turretsProejctile, turret.GunTip.position, turret.GunTip.rotation);
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

        public override void LevelUp()
        {
            _level++;
            OnLevelUp?.Invoke(this, EventArgs.Empty);
        }
    }
}
