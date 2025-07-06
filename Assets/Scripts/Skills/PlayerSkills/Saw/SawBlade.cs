using Assets.ScriptableObjects.Skills.PlayerSkills.SawSkill;
using Assets.Scripts.Initializers;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.Saw
{
    public class SawBlade : MonoBehaviour, IInitializableWithScriptableConfig<SawSkillUpgradeableConfigSO>
    {
        private const float COLLISION_CHECK_INTERVAL = 0.1f;
        private SawSkillUpgradeableConfigSO _config;
        private BoxCollider _boxCollider;
        private bool _isInitialized;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        public void OnTriggerEnter(Collider other)
        {
            AttackCollidingEntity(other);
        }

        public void Initialize(SawSkillUpgradeableConfigSO config)
        {
            _config = config;

            gameObject.SetActive(true);

            _isInitialized = true;

            InvokeRepeating(nameof(AtackAllEnemiesInsideCollider), 0, COLLISION_CHECK_INTERVAL);
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }

        private void AtackAllEnemiesInsideCollider()
        {
            Vector3 boxWorldCenter = transform.TransformPoint(_boxCollider.center);

            Collider[] colliders = Physics.OverlapBox(
                boxWorldCenter,
                _boxCollider.size * 0.5f,
                transform.rotation,
                EntityLayers.All);

            foreach (Collider collider in colliders)
            {
                AttackCollidingEntity(collider);
            }
        }

        private void AttackCollidingEntity(Collider other)
        {
            GameObject collisionObject = other.transform.gameObject;
            if (1 << collisionObject.layer == EntityLayers.Enemy)
            {
                EntityManipulationHelper.Damage(other, _config.Damage.Value);

                EntityManipulationHelper.Knockback(
                    other,
                    transform.forward,
                    _config.KnockbackRange.Value,
                    _config.TimeToArriveAtKnockbackLocation);

                EntityManipulationHelper.Stun(other, _config.StunDuration.Value);
            }
        }
    }
}
