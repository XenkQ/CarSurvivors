using Assets.Scripts.Collisions;
using Assets.Scripts.HealthSystem;
using Assets.Scripts.Player;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class Enemy : MonoBehaviour, IHealthy, IDamageable, IKnockable, IStunnable
    {
        [field: SerializeField] public EnemyConfigSO Config { get; private set; }

        public IHealth Health { get; private set; }

        public IStunController StunController { get; private set; }

        public ICollisionsController CollisionsController { get; private set; }

        public IMovementController MovementController { get; private set; }

        public EnemyAnimator EnemyAnimator { get; private set; }

        private void Awake()
        {
            Health = GetComponent<IHealth>();
            StunController = GetComponent<IStunController>();
            CollisionsController = GetComponent<ICollisionsController>();
            MovementController = GetComponent<IMovementController>();
            EnemyAnimator = GetComponentInChildren<EnemyAnimator>();
        }

        private void Start()
        {
            Health.MaxHealth = Config.MaxHealth;
        }

        private void OnEnable()
        {
            EnemyAnimator.IsMovingByCrawling = Config.IsMovingByCrawling;
        }

        private void OnDisable()
        {
            PlayerManager.Instance.LevelController.AddExp(Config.ExpForKill);
        }

        public void ApplyKnockBack(Vector3 direction, float power, float timeToArriveAtLocation)
        {
            MovementController.MoveToPositionInTimeIgnoringSpeed(transform.position + (direction * power), timeToArriveAtLocation);
        }

        public void TakeDamage(float damage)
        {
            Health.DecreaseHealth(damage);
        }

        public void ApplyStun(float duration)
        {
            StunController.PerformStun(duration);
        }
    }
}
