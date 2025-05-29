using Assets.Scripts.HealthSystem;
using Assets.Scripts.LayerMasks;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills
{
    public class SawSkill : UpgradeableSkill<SawSkillUpgradeableConfigSO>
    {
        [field: SerializeField] public override SawSkillUpgradeableConfigSO Config { get; protected set; }
        [SerializeField] private BoxCollider _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        public void OnTriggerEnter(Collider other)
        {
            AttackCollidingEntity(other);
        }

        public override void Initialize()
        {
            base.Initialize();

            InvokeRepeating(nameof(AtackAllEnemiesInsideCollider), Config.AttackCooldown.Value, Config.AttackCooldown.Value);
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
                    damageable.TakeDamage(Config.Damage.Value);
                }

                if (collisionObject.TryGetComponent(out IKnockable knockable))
                {
                    knockable.ApplyKnockBack(
                        other.transform.position + transform.forward * Config.KnockbackPower.Value,
                        Config.TimeToArriveAtKnockbackLocation.Value);
                }
            }
        }
    }
}
