using Assets.ScriptableObjects.Skills.PlayerSkills.SawSkill;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.Saw
{
    public class SawBlade : MonoBehaviour, IInitializableWithScriptableConfig<SawSkillUpgradeableConfigSO>
    {
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

            InvokeRepeating(nameof(AtackAllEnemiesInsideCollider), 0.05f, 0.05f);
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }

        private void AtackAllEnemiesInsideCollider()
        {
            Collider[] colliders = Physics.OverlapBox(
                transform.position + _boxCollider.center,
                _boxCollider.size,
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
                if (collisionObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_config.Damage.Value);
                }

                if (collisionObject.TryGetComponent(out IKnockable knockable))
                {
                    knockable.ApplyKnockBack(
                        transform.forward,
                        _config.KnockbackPower.Value,
                        _config.TimeToArriveAtKnockbackLocation);
                }
            }
        }
    }
}
