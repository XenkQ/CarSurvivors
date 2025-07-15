using Assets.ScriptableObjects.Skills.PlayerSkills.SawSkill;
using Assets.Scripts.Initializers;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.Player;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.Saw
{
    public class SawBlade : MonoBehaviour, IInitializableWithScriptableConfig<SawSkillUpgradeableConfigSO>
    {
        private SawSkillUpgradeableConfigSO _config;
        private bool _isInitialized;
        private PlayerManager _playerManager;
        private const float _defaultCollisionKnockback = 2f;

        private void Start()
        {
            _playerManager = PlayerManager.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((1 << other.gameObject.layer) == EntityLayers.Enemy)
            {
                AttackCollidingEnemy(other);
            }
        }

        public void Initialize(SawSkillUpgradeableConfigSO config)
        {
            _config = config;

            gameObject.SetActive(true);

            _isInitialized = true;
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }

        private void AttackCollidingEnemy(Collider other)
        {
            EntityManipulationHelper.Damage(other, _config.Damage.Value);

            float knockback = Mathf.Max(
                _defaultCollisionKnockback,
                _config.KnockbackRange.Value * _playerManager.CarController.GetMovementSpeed()
            );

            EntityManipulationHelper.Knockback(
                other,
                transform.forward,
                knockback,
                _config.TimeToArriveAtKnockbackLocation);

            EntityManipulationHelper.Stun(other, _config.TimeToArriveAtKnockbackLocation);
        }
    }
}
