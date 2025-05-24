using Assets.Scripts.Player.Skills;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.LandmineTrap
{
    public class LandmineSkill : Skill<LandmineSkillConfigSO>
    {
        [field: SerializeField] public override StartEndScriptableConfig<LandmineSkillConfigSO> StartEndScriptableConfig { get; protected set; }
        [SerializeField] private Landmine _landminePrefab;
        [SerializeField] private Transform _landminesParent;
        [SerializeField] private float _cooldown;
        public override LandmineSkillConfigSO CurrentConfig { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            InvokeRepeating(nameof(SpawnLandmine), CurrentConfig.SpawnCooldown, CurrentConfig.SpawnCooldown);
        }

        private void SpawnLandmine()
        {
            Landmine landmine = Instantiate(_landminePrefab, transform.position, Quaternion.identity, _landminesParent);
            landmine.Initialize(CurrentConfig);
        }
    }
}
