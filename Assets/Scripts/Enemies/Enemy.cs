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

        private ICollisionsController _enemyCollisions;

        private IMovementController _enemyMovement;

        private EnemyAnimationsController _enemyAnimationsController;

        private void Awake()
        {
            Health = GetComponent<IHealth>();
            StunController = GetComponent<IStunController>();
            _enemyCollisions = GetComponent<ICollisionsController>();
            _enemyMovement = GetComponent<IMovementController>();
            _enemyAnimationsController = GetComponent<EnemyAnimationsController>();
        }

        private void OnEnable()
        {
            _enemyCollisions.OnCollisionWithPlayer += EnemyCollisions_OnCollisionWithPlayer;
        }

        private void OnDisable()
        {
            _enemyCollisions.OnCollisionWithPlayer -= EnemyCollisions_OnCollisionWithPlayer;

            Player.PlayerManager.Instance.LevelController.AddExp(Config.ExpForKill);
        }

        public void ApplyKnockBack(Vector3 locationAfterKnockBack, float timeToArriveAtLocation)
        {
            Debug.Log("KNOCKBACK");
            _enemyMovement.MoveToPosition(locationAfterKnockBack);
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
                _enemyAnimationsController.PlayAttackAnimation();
                damageable.TakeDamage(Config.Damage);
            }
        }
    }
}
