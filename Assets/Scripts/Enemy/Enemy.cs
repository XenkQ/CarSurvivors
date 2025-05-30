using Assets.Scripts.Exp;
using Assets.Scripts.GridSystem;
using Assets.Scripts.HealthSystem;
using Assets.Scripts.LayerMasks;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(Health), typeof(Collider))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        public Health Health { get; private set; }

        private const float PUSH_FROM_COLLISION_POWER = 1f;

        [SerializeField] private EnemyStatsSO _stats;
        [SerializeField] private GrowShrinkAnimation _growShrinkAnimation;

        [SerializeField] private float _collisionRadius;
        private Collider _collider;

        private float _verticalPosOffset;
        private float _currentStunAfterDamageDelay;

        private void Awake()
        {
            Health = GetComponent<Health>();
            _collider = GetComponent<Collider>();
            _growShrinkAnimation.Initialize(_stats.GrowShrinkAnimationConfiguration);
        }

        private void OnEnable()
        {
            _verticalPosOffset = transform.position.y;

            _growShrinkAnimation.StartAnimation();

            Health.OnNoHealth += Health_OnNoHealth;
        }

        private void OnDisable()
        {
            transform.position = new Vector3(0, _verticalPosOffset, 0);

            _growShrinkAnimation.StopAnimation();

            Health.OnNoHealth -= Health_OnNoHealth;
        }

        private void Update()
        {
            if (_currentStunAfterDamageDelay > 0)
            {
                _currentStunAfterDamageDelay -= Time.deltaTime;
            }
            else
            {
                Vector3 movement = MoveOnGrid();
                RotateTowardsMovementDirection(movement);
            }
        }

        private void FixedUpdate()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _collisionRadius, Vector3.up, Mathf.Infinity, EntityLayers.All);
            foreach (var hit in hits)
            {
                Collider collider = hit.collider;
                if (hit.collider != null && hit.collider != _collider)
                {
                    if (1 << collider.gameObject.layer == EntityLayers.Enemy)
                    {
                        PreventInterectingWithColliderByPush(collider);
                    }
                    else if (1 << collider.gameObject.layer == EntityLayers.Player
                             && collider.gameObject.TryGetComponent(out IDamageable damagable))
                    {
                        damagable.TakeDamage(_stats.Damage);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            const int numberOfSegments = 16;
            DebugUtilities.DrawCircle(transform.position, _collisionRadius, numberOfSegments, Color.yellow);
        }

        private void Health_OnNoHealth(object sender, System.EventArgs e)
        {
            ExpManager.Instance.AddExp(_stats.ExpFromKill);
        }

        public void TakeDamage(float damage)
        {
            _currentStunAfterDamageDelay = _stats.StunAfterDamageDuration;
            Health.DecreaseHealth(damage);
        }

        private Vector3 MoveOnGrid()
        {
            Vector3 gridDir = GetMoveDirectionBasedOnCurrentCell();
            Vector3 movement = _stats.MovementSpeed * Time.deltaTime * gridDir;
            transform.position += movement;
            return movement;
        }

        private Vector3 GetMoveDirectionBasedOnCurrentCell()
        {
            GridSystem.Grid grid = GridManager.Instance.WorldGrid;
            Cell currentCell = grid.GetCellFromWorldPos(transform.position);
            if (currentCell != null && currentCell.BestDirection != null)
            {
                Vector2Int gridDirection = currentCell.BestDirection.Vector;
                return new Vector3(gridDirection.x, 0, gridDirection.y);
            }

            return Vector3.zero;
        }

        private void RotateTowardsMovementDirection(Vector3 movement)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _stats.RotationSpeed * Time.deltaTime);
        }

        private void PreventInterectingWithColliderByPush(Collider collider)
        {
            Vector3 otherEnemyDirection = (collider.transform.position - transform.position).normalized;
            Vector3 pushDestination = transform.position + -otherEnemyDirection * PUSH_FROM_COLLISION_POWER;
            pushDestination.y = _verticalPosOffset;
            transform.position = Vector3.Lerp(transform.position, pushDestination, _stats.MovementSpeed * Time.deltaTime);
        }
    }
}
