using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyStuckInsideOtherEnemyPreventer : MonoBehaviour
    {
        private const float PUSH_FROM_COLLISION_POWER = 1f;
        [SerializeField] private float _stuckCheckDelay = 0.1f;
        private EnemyMovement _enemyMovement;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
        }

        public void PreventInterectingWithColliderByPush(Collider collider)
        {
            float verticalPos = transform.position.y;
            Vector3 otherEnemyDirection = (collider.transform.position - transform.position).normalized;
            Vector3 pushDestination = transform.position + -otherEnemyDirection * PUSH_FROM_COLLISION_POWER;
            pushDestination.y = verticalPos;
            _enemyMovement.MoveToPosition(pushDestination);
        }
    }
}
