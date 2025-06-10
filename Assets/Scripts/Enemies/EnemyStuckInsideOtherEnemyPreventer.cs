using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyStuckInsideOtherEnemyPreventer : MonoBehaviour
    {
        private const float PUSH_FROM_COLLISION_POWER = 1f;
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private float _stuckCheckDelay = 0.1f;

        public void PreventInterectingWithColliderByPush(Collider collider)
        {
            float verticalPos = transform.position.y;
            Vector3 otherEnemyDirection = (collider.transform.position - transform.position).normalized;
            Vector3 pushDestination = transform.position + -otherEnemyDirection * PUSH_FROM_COLLISION_POWER;
            pushDestination.y = verticalPos;
            transform.position = Vector3.Lerp(transform.position, pushDestination, _stats.MovementSpeed * Time.deltaTime);
        }
    }
}
