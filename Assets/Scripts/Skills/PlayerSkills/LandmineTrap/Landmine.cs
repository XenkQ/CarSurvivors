using Assets.ScriptableObjects.Skills.PlayerSkills.LandmineSkill;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts.Skills.PlayerSkills.LandmineTrap
{
    public class Landmine : MonoBehaviour, IInitializableWithScriptableConfig<LandmineSkillUpgradeableConfigSO>
    {
        [SerializeField] private LandmineSkillUpgradeableConfigSO _config;
        private bool _isInitialized;

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

        public void Initialize(LandmineSkillUpgradeableConfigSO config)
        {
            _config = config;
        }

        public bool IsInitialized() => _isInitialized;
    }
}
