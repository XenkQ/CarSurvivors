using Assets.ScriptableObjects.Player.Skills;
using Assets.Scripts.Player.Skills;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.LandmineTrap
{
    public class LandmineSkill : Skill<LandmineSkillConfigSO>
    {
        [field: SerializeField] public override StartEndScriptableConfig<SkillConfig> StartEndScriptableConfig { get; protected set; }
        [SerializeField] private Landmine _landminePrefab;
        [SerializeField] private Transform _landminesParent;
        [SerializeField] private float _cooldown;

        public override void Initialize()
        {
            base.Initialize();

            InvokeRepeating(nameof(SpawnLandmine), _currentConfig.SpawnCooldown, _currentConfig.SpawnCooldown);
        }

        private void SpawnLandmine()
        {
            Landmine landmine = Instantiate(_landminePrefab, transform.position, Quaternion.identity, _landminesParent);
            landmine.Initialize(_currentConfig);
        }
    }
}
