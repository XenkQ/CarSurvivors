using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.LandmineTrap
{
    public class LandmineSkill : ConfigurableSkill<LandmineSkillUpgradableConfigSO>
    {
        [field: SerializeField] public override LandmineSkillUpgradableConfigSO Config { get; protected set; }
        [SerializeField] private Landmine _landminePrefab;
        [SerializeField] private Transform _landminesParent;
        [SerializeField] private float _cooldown;

        public override void Initialize()
        {
            base.Initialize();

            InvokeRepeating(nameof(SpawnLandmine), Config.SpawnCooldown.Value, Config.SpawnCooldown.Value);
        }

        private void SpawnLandmine()
        {
            Landmine landmine = Instantiate(_landminePrefab, transform.position, Quaternion.identity, _landminesParent);
            landmine.Initialize(Config);
        }
    }
}
