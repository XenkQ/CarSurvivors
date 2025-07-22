using Assets.Scripts.Audio;
using Assets.Scripts.Collisions;
using Assets.Scripts.DamagePopups;
using Assets.Scripts.HealthSystem;
using Assets.Scripts.StatusAffectables;
using System;
using UnityEngine;
using VFX;

namespace Assets.Scripts.Enemies
{
    public class Enemy : MonoBehaviour, IHealthy, IDamageable, IKnockable, IStunnable, IPoolable
    {
        [field: SerializeField] public EnemyConfigSO Config { get; private set; }
        [SerializeField] private VFXPlayer _bloodVfxPlayer;
        [SerializeField] private GameObject _visual;

        public IHealth Health { get; private set; }
        public IStunController StunController { get; private set; }
        public ICollisionsController CollisionsController { get; private set; }
        public IMovementController MovementController { get; private set; }
        public IAudioClipPlayer AudioClipPlayer { get; private set; }
        public EnemyAnimator EnemyAnimator { get; private set; }

        public event EventHandler OnCanBeReleased;

        private IDamagePopupsSpawner _damagePopupsSpawner;
        private INeedToCompleteBeforeDisable _enemyDeathSequence;

        private void Awake()
        {
            Health = GetComponent<IHealth>();
            StunController = GetComponent<IStunController>();
            CollisionsController = GetComponent<ICollisionsController>();
            MovementController = GetComponent<IMovementController>();
            AudioClipPlayer = GetComponentInChildren<IAudioClipPlayer>();
            _enemyDeathSequence = GetComponent<INeedToCompleteBeforeDisable>();
            EnemyAnimator = GetComponentInChildren<EnemyAnimator>();
        }

        private void Start()
        {
            _damagePopupsSpawner = DamagePopupsSpawner.Instance;
        }

        public void OnGet()
        {
            _enemyDeathSequence.OnCompleted += EnemyDeathSequence_OnCompleted;

            _visual.SetActive(true);

            Health.MaxHealth = Config.MaxHealth;
        }

        public void OnRelease()
        {
            _enemyDeathSequence.OnCompleted -= EnemyDeathSequence_OnCompleted;
        }

        public void ApplyKnockBack(Vector3 direction, float power, float timeToArriveAtLocation)
        {
            MovementController.MoveToPositionInTimeIgnoringSpeed(transform.position + (direction * power), timeToArriveAtLocation);
        }

        public void TakeFullHpDamage()
        {
            Health.DecreaseHealth(Health.MaxHealth);
        }

        public void TakeDamage(float damage)
        {
            _damagePopupsSpawner.SpawnDamagePopup(
                _bloodVfxPlayer.transform.position,
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

        private void EnemyDeathSequence_OnCompleted(object sender, EventArgs e)
        {
            OnCanBeReleased?.Invoke(this, EventArgs.Empty);
        }
    }
}
