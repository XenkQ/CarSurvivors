using Assets.ScriptableObjects.Skills;
using Assets.ScriptableObjects.Skills.PlayerSkills.LandmineSkill;
using Assets.Scripts.LayerMasks;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.LandmineTrap
{
    public class LandmineSkill : UpgradeableSkill<LandmineSkillUpgradeableConfigSO>
    {
        private const float CAN_PLACE_MINE_RAY_DISTANCE = 5f;

        [field: SerializeField] public override SkillInfoSO SkillInfo { get; protected set; }
        [field: SerializeField] protected override LandmineSkillUpgradeableConfigSO _config { get; set; }

        [SerializeField] private Landmine _landminePrefab;
        [SerializeField] private Transform _landminesParent;
        [SerializeField] private float _cooldown;

        public override void Initialize()
        {
            base.Initialize();

            InvokeRepeating(nameof(SpawnLandmine), 0, _config.SpawnCooldown.Value);
        }

        private void SpawnLandmine()
        {
            if (Physics.Raycast(transform.position, Vector3.down, CAN_PLACE_MINE_RAY_DISTANCE, TerrainLayers.Ground))
            {
                Landmine landmine = Instantiate(
                    _landminePrefab,
                    transform.position,
                    Quaternion.identity,
                    _landminesParent
                );

                landmine.Initialize(_config);
            }
        }
    }
}
