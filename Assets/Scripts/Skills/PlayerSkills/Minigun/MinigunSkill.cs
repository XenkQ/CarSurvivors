using Assets.ScriptableObjects;
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
        private IItemsWithScriptableConfigsActivator<MinigunTurret, TurretConfigSO> _turretsActivator;

        public override void Initialize()
        {
            base.Initialize();

            _turretsActivator =
                new ItemsWithScriptableConfigsActivator<MinigunTurret, TurretConfigSO>(_turrets);

            _config.NumberOfTurrets.OnUpgrade += (s, e) =>
                _turretsActivator.InitializeRandom(_config.TurretConfig);

            StartCoroutine(SpawnBulletsProcess());
        }

        private System.Collections.IEnumerator SpawnBulletsProcess()
        {
            while (true)
            {
                foreach (MinigunTurret turret in _turretsActivator.GetInitialized())
                {
                    Projectile projectile = Instantiate(_turretsProejctile, turret.GunTip.position, turret.GunTip.rotation, _projectilesParent);
                    projectile.OnLifeEnd += Projectile_OnLifeEnd;
                    projectile.Initialize(_config.TurretConfig.ProjectileStatsSO);
                }

                yield return new WaitForSeconds(_config.DelayBetweenSpawningBullets.Value);
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
