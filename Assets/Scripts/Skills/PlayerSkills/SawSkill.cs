using Assets.ScriptableObjects.Skills;
using Assets.ScriptableObjects.Skills.PlayerSkills.SawSkill;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills
{
    public class SawSkill : UpgradeableSkill<SawSkillUpgradeableConfigSO>
    {
        [field: SerializeField] public override SkillInfoSO SkillInfo { get; protected set; }
        [field: SerializeField] protected override SawSkillUpgradeableConfigSO _config { get; set; }
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

            InvokeRepeating(nameof(AtackAllEnemiesInsideCollider), 0.05f, 0.05f);
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
