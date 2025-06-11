using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(EnemyMovementController), typeof(EnemyCollisionsController))]
    public class EnemyStuckInsideOtherEnemyPreventer : MonoBehaviour
    {
        private const float PUSH_FROM_COLLISION_POWER = 1f;
        private EnemyMovementController _enemyMovement;
        private EnemyCollisionsController _enemyCollisions;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovementController>();
            _enemyCollisions = GetComponent<EnemyCollisionsController>();
        }

        private void OnEnable()
        {
            _enemyCollisions.OnCollisionWithOtherEnemy += EnemyCollisions_OnCollisionWithOtherEnemy; ;
        }

        private void OnDisable()
        {
            _enemyCollisions.OnCollisionWithOtherEnemy -= EnemyCollisions_OnCollisionWithOtherEnemy;
        }

        private void EnemyCollisions_OnCollisionWithOtherEnemy(object sender, CollisionEventArgs e)
        {
            PreventInterectingWithColliderByPush(e.Collider);
        }

        private void PreventInterectingWithColliderByPush(Collider collider)
        {
            float verticalPos = transform.position.y;
            Vector3 otherEnemyDirection = (collider.transform.position - transform.position).normalized;
            Vector3 pushDestination = transform.position + -otherEnemyDirection * PUSH_FROM_COLLISION_POWER;
            pushDestination.y = verticalPos;
            _enemyMovement.MoveToPosition(pushDestination);
        }
    }
}
