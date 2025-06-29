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

            DamageAllEnemiesInExplosionRadius();
        }

        private void OnDrawGizmos()
        {
            if (_config is not null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _config.ExplosionRadius.Value);
            }
        }

        private void DamageAllEnemiesInExplosionRadius()
        {
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

        public void Initialize(LandmineSkillUpgradeableConfigSO config)
        {
            _config = config;

            transform.localScale = new Vector3(_config.Size.Value, transform.localScale.y, _config.Size.Value);

            gameObject.SetActive(true);
        }

        public bool IsInitialized() => _isInitialized;
    }
}
