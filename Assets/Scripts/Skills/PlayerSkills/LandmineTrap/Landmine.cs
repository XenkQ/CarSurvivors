using Assets.Scripts.HealthSystem;
using Assets.Scripts.LayerMasks;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.LandmineTrap
{
    public class Landmine : MonoBehaviour, IInitializable<LandmineSkillConfigSO>
    {
        [SerializeField] private LandmineSkillConfigSO _config;

        private void OnTriggerEnter(Collider other)
        {
            if (1 << other.gameObject.layer != EntityLayers.Enemy)
            {
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, _config.ExplosionRadius.Value, EntityLayers.Enemy, QueryTriggerInteraction.Collide);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_config.Damage.Value);
                    Destroy(gameObject);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _config.ExplosionRadius.Value);
        }

        public void Initialize(LandmineSkillConfigSO config)
        {
            _config = config;
        }

        public bool IsInitialized()
        {
            return gameObject.activeSelf;
        }
    }
}
