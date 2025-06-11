using Assets.Scripts.Collisions;
using Assets.Scripts.HealthSystem;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class Enemy : MonoBehaviour, IHealthy, IDamageable, IKnockable, IStunable
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

        private void OnEnable()
        {
            EnemyAnimator.IsMovingByCrawling = Config.IsMovingByCrawling;
            CollisionsController.OnCollisionWithPlayer += EnemyCollisions_OnCollisionWithPlayer;
        }

        private void OnDisable()
        {
            CollisionsController.OnCollisionWithPlayer -= EnemyCollisions_OnCollisionWithPlayer;

            Player.PlayerManager.Instance.LevelController.AddExp(Config.ExpForKill);
        }

        public void ApplyKnockBack(Vector3 locationAfterKnockBack, float timeToArriveAtLocation)
        {
            Debug.Log("KNOCKBACK");
            MovementController.MoveToPosition(locationAfterKnockBack);
        }

        public void TakeDamage(float damage)
        {
            StunController.ApplyStun();
            Health.DecreaseHealth(damage);
        }

        private void EnemyCollisions_OnCollisionWithPlayer(object sender, CollisionEventArgs e)
        {
            if (e.Collider.TryGetComponent(out IDamageable damageable))
            {
                EnemyAnimator.PlayAttackAnimation();
                damageable.TakeDamage(Config.Damage);
            }
        }
    }
}
