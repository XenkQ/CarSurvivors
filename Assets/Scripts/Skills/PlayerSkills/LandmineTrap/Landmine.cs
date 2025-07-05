using Assets.ScriptableObjects.Skills.PlayerSkills.LandmineSkill;
using Assets.Scripts.Initializers;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.StatusAffectables;
using UnityEngine;
using VFX;

namespace Assets.Scripts.Skills.PlayerSkills.LandmineTrap
{
    public class Landmine : MonoBehaviour, IInitializableWithScriptableConfig<LandmineSkillUpgradeableConfigSO>
    {
        [SerializeField] private LandmineSkillUpgradeableConfigSO _config;
        [SerializeField] private GameObject _landmineVisual;
        [SerializeField] private VFXPlayer _deathVfxPlayer;
        private bool _isInitialized;

        private void OnTriggerEnter(Collider other)
        {
            if (!_landmineVisual.activeSelf || 1 << other.gameObject.layer != EntityLayers.Enemy)
            {
                return;
            }

            Explode();
        }

        private void OnDrawGizmos()
        {
            if (_config is not null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _config.ExplosionRadius.Value);
            }
        }

        private void OnEnable()
        {
            _deathVfxPlayer.OnVFXFinished += DeathVfxPlayer_OnVFXFinished;
            _landmineVisual.SetActive(true);
        }

        private void OnDisable()
        {
            _deathVfxPlayer.OnVFXFinished -= DeathVfxPlayer_OnVFXFinished;
        }

        private void DeathVfxPlayer_OnVFXFinished(object sender, System.EventArgs e)
        {
            Destroy(gameObject);
        }

        private void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _config.ExplosionRadius.Value, EntityLayers.Enemy, QueryTriggerInteraction.Collide);

            if (colliders.Length == 0)
            {
                return;
            }

            _deathVfxPlayer.Play();

            foreach (Collider collider in colliders)
            {
                ApplyDamageOnDamageableEntity(collider);

                ApplyExplosionKnockbackOnKnockableEntity(collider);
            }

            _landmineVisual.SetActive(false);
        }

        private void ApplyDamageOnDamageableEntity(Collider collider)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_config.Damage.Value);
            }
        }

        private void ApplyExplosionKnockbackOnKnockableEntity(Collider collider)
        {
            if (collider.TryGetComponent(out IKnockable knockable))
            {
                Vector3 dir = (collider.transform.position - transform.position).normalized;
                float timeToArriveAtLocation = 1f / _config.KnockbackRange.Value;

                knockable.ApplyKnockBack(dir, _config.KnockbackRange.Value, timeToArriveAtLocation);
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
