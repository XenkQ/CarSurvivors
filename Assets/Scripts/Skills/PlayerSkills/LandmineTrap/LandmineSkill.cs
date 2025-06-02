using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.LandmineTrap
{
    public class LandmineSkill : UpgradeableSkill<LandmineSkillUpgradeableConfigSO>
    {
        [field: SerializeField] public override SkillInfoSO SkillInfo { get; protected set; }
        [field: SerializeField] protected override LandmineSkillUpgradeableConfigSO _config { get; set; }

        [SerializeField] private Landmine _landminePrefab;
        [SerializeField] private Transform _landminesParent;
        [SerializeField] private float _cooldown;

        public override void Initialize()
        {
            base.Initialize();

            InvokeRepeating(nameof(SpawnLandmine), _config.SpawnCooldown.Value, _config.SpawnCooldown.Value);
        }

        private void SpawnLandmine()
        {
            Landmine landmine = Instantiate(_landminePrefab, transform.position, Quaternion.identity, _landminesParent);
            landmine.Initialize(_config);
        }
    }
}
