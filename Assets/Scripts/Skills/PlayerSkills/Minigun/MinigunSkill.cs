using Assets.ScriptableObjects.Skills;
using Assets.ScriptableObjects.Skills.PlayerSkills.MinigunSkill;
using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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

            StartCoroutine(SpawnBulletsProcess());

            InitializeActiveUninitializedTurrets();

            _config.NumberOfTurrets.OnUpgrade += (s, e) => ActiveRandomInactiveTurret()?.Initialize(_config.TurretConfig);
            _config.NumberOfTurrets.OnUpgrade += (s, e) => Debug.Log("UPGRADE");
        }

        private System.Collections.IEnumerator SpawnBulletsProcess()
        {
            while (true)
            {
                foreach (MinigunTurret turret in GetActiveInitializedTurrets())
                {
                    Projectile projectile = Instantiate(_turretsProejctile, turret.GunTip.position, turret.GunTip.rotation, _projectilesParent);
                    projectile.OnLifeEnd += Projectile_OnLifeEnd;
                    projectile.Initialize(_config.TurretConfig.ProjectileStatsSO);
                }

                yield return new WaitForSeconds(_config.DelayBetweenSpawningBullets.Value);
            }
        }

        private void InitializeActiveUninitializedTurrets()
        {
            foreach (MinigunTurret turret in GetActiveUninitializedTurrets())
            {
                turret.Initialize(_config.TurretConfig);
            }
        }

        private MinigunTurret ActiveRandomInactiveTurret()
        {
            var inactiveTurrets = _turrets.Where(t => !t.gameObject.activeSelf);
            Debug.Log(inactiveTurrets.Count());
            if (inactiveTurrets.Any())
            {
                MinigunTurret turret = inactiveTurrets.Shuffle().First();
                turret.gameObject.SetActive(true);
                return turret;
            }

            return null;
        }

        private IEnumerable<MinigunTurret> GetActiveInitializedTurrets()
            => _turrets.Where(t => t.IsInitialized() && t.gameObject.activeSelf);

        private IEnumerable<MinigunTurret> GetActiveUninitializedTurrets() =>
            _turrets.Where(t => !t.IsInitialized() && t.gameObject.activeSelf);

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
