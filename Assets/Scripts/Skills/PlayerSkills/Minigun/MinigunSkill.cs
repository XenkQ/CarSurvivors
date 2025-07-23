﻿using Assets.ScriptableObjects;
using Assets.ScriptableObjects.Skills;
using Assets.ScriptableObjects.Skills.PlayerSkills.MinigunSkill;
using Assets.Scripts.Activators;
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

            StartCoroutine(SpawnProjectilesProcess());
        }

        private IEnumerator SpawnProjectilesProcess()
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
    }
}
