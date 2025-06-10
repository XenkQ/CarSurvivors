using Assets.Scripts.Extensions;
using Assets.Scripts.HealthSystem;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable, IKnockable, IStunable
    {
        [SerializeField] private EnemyConfigSO _config;

        [SerializeField] private float _collisionRadius;
        public Health Health { get; private set; }

        public StunController StunController { get; private set; }

        private EnemyCollisions _enemyCollisions;

        private EnemyStuckInsideOtherEnemyPreventer _enemyStuckInsideOtherEnemyPreventer;

        public bool IsStunned;

        private void Awake()
        {
            Health = GetComponent<Health>();
            _enemyCollisions = GetComponent<EnemyCollisions>();
            _enemyStuckInsideOtherEnemyPreventer = GetComponent<EnemyStuckInsideOtherEnemyPreventer>();
            StunController = GetComponent<StunController>();
        }

        private void OnEnable()
        {
            _enemyCollisions.OnCollisionWithPlayer += EnemyCollisions_OnCollisionWithPlayer;
            _enemyCollisions.OnCollisionWithOtherEnemy += EnemyCollisions_OnCollisionWithOtherEnemy;
        }

        private void OnDisable()
        {
            Player.PlayerManager.Instance.LevelController.AddExp(_config.ExpForKill);
        }

        private void OnDrawGizmos()
        {
            int numberOfSegments = 16;
            new Debug().DrawCircle(transform.position, _collisionRadius, numberOfSegments, Color.yellow);
        }

        private void EnemyCollisions_OnCollisionWithPlayer(object sender, CollisionEventArgs e)
        {
            if (e.Collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_config.Damage);
            }
        }

        private void EnemyCollisions_OnCollisionWithOtherEnemy(object sender, CollisionEventArgs e)
        {
            _enemyStuckInsideOtherEnemyPreventer.PreventInterectingWithColliderByPush(e.Collider);
        }

        public void ApplyKnockBack(Vector3 locationAfterKnockBack, float timeToArriveAtLocation)
        {
            transform.DOMove(locationAfterKnockBack, timeToArriveAtLocation);
        }

        public void TakeDamage(float damage)
        {
            StunController.ApplyStun();
            Health.DecreaseHealth(damage);
        }
    }
}
