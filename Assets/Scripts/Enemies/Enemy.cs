using Assets.Scripts.Collisions;
using Assets.Scripts.DamagePopups;
using Assets.Scripts.HealthSystem;
using Assets.Scripts.LevelSystem.Exp;
using Assets.Scripts.StatusAffectables;
using UnityEngine;
using VFX;

namespace Assets.Scripts.Enemies
{
    public class Enemy : MonoBehaviour, IHealthy, IDamageable, IKnockable, IStunnable, IPoolable
    {
        [field: SerializeField] public EnemyConfigSO Config { get; private set; }
        [SerializeField] private VFXPlayer _bloodVfxPlayer;
        [SerializeField] private VFXPlayer _deathVfxPlayer;
        [SerializeField] private Vector3 _deathEffectCenterOffset;

        public IHealth Health { get; private set; }

        public IStunController StunController { get; private set; }

        public ICollisionsController CollisionsController { get; private set; }

        public IMovementController MovementController { get; private set; }

        public EnemyAnimator EnemyAnimator { get; private set; }

        private IDamagePopupsSpawner _damagePopupsSpawner;

        private void Awake()
        {
            Health = GetComponent<IHealth>();
            StunController = GetComponent<IStunController>();
            CollisionsController = GetComponent<ICollisionsController>();
            MovementController = GetComponent<IMovementController>();
            EnemyAnimator = GetComponentInChildren<EnemyAnimator>();
        }

        private void OnEnable()
        {
            Health.MaxHealth = Config.MaxHealth;

            EnemyAnimator.IsMovingByCrawling = Config.IsMovingByCrawling;
        }

        private void Start()
        {
            _damagePopupsSpawner = DamagePopupsSpawner.Instance;
        }

        public void OnGet()
        {
            gameObject.SetActive(true);
        }

        public void OnRelease()
        {
            SpawnDeathParticles();

            SpawnExp();

            gameObject.SetActive(false);
        }

        public void ApplyKnockBack(Vector3 direction, float power, float timeToArriveAtLocation)
        {
            MovementController.MoveToPositionInTimeIgnoringSpeed(transform.position + (direction * power), timeToArriveAtLocation);
        }

        public void InstantKill()
        {
            Health.DecreaseHealth(Health.MaxHealth);
        }

        public void TakeDamage(float damage)
        {
            _damagePopupsSpawner.SpawnDamagePopup(
                transform.position + _deathEffectCenterOffset,
                damage,
                SpawnShapeModes.Hemisphere
            );

            Health.DecreaseHealth(damage);

            if (Health.IsAlive())
            {
                _bloodVfxPlayer.Play(new VFXPlayConfig());
            }
        }

        public void ApplyStun(float duration)
        {
            StunController.PerformStun(duration);
        }

        private void SpawnDeathParticles()
        {
            var deathVfxPlayer = Instantiate(_deathVfxPlayer, transform.position + _deathEffectCenterOffset, Quaternion.identity);

            deathVfxPlayer.Play(new VFXPlayConfig(destroyOnEnd: true));
        }

        private void SpawnExp()
        {
            ExpParticleSpawner.Instance.SpawnExpParticle(
                new ExpParticleSpawner.ExpParticleSpawnData(
                    transform.position + _deathEffectCenterOffset,
                    Config.ExpForKill
                )
            );
        }
    }
}
