using Assets.ScriptableObjects;
using Assets.ScriptableObjects.Skills;
using Assets.ScriptableObjects.Skills.PlayerSkills.MinigunSkill;
using Assets.Scripts.Activators;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.Minigun
{
    public class MinigunSkill : UpgradeableSkill<MinigunSkillUpgradeableConfigSO>
    {
        [field: SerializeField] public override SkillInfoSO SkillInfo { get; protected set; }
        [field: SerializeField] protected override MinigunSkillUpgradeableConfigSO _config { get; set; }
        [SerializeField] private MinigunTurret[] _turrets;
        private IItemsWithScriptableConfigsActivator<MinigunTurret, TurretConfigSO> _turretsActivator;

        public override void Initialize()
        {
            base.Initialize();

            _turretsActivator =
                new ItemsWithScriptableConfigsActivator<MinigunTurret, TurretConfigSO>(_turrets);

            _turretsActivator.InitializeRandom(_config.TurretConfig);

            _config.NumberOfTurrets.OnUpgrade += (s, e) =>
                _turretsActivator.InitializeRandom(_config.TurretConfig);

            StartCoroutine(SpawnBulletsProcess());
        }

        private IEnumerator SpawnBulletsProcess()
        {
            while (true)
            {
                foreach (MinigunTurret turret in _turretsActivator.GetInitialized())
                {
                    turret.Shoot();
                }

                yield return new WaitForSeconds(_config.DelayBetweenShoots.Value);
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
