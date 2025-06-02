using Assets.Scripts.Extensions;
using Assets.Scripts.GridSystem;
using Assets.Scripts.HealthSystem;
using Assets.Scripts.LayerMasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Health), typeof(Collider))]
    public class Enemy : MonoBehaviour, IDamageable, IKnockable
    {
        public Health Health { get; private set; }

        private const float PUSH_FROM_COLLISION_POWER = 1f;

        [SerializeField] private EnemyConfigSO _stats;

        [SerializeField] private float _collisionRadius;
        private Collider _collider;

        private float _verticalPosOffset;
        private float _currentStunAfterDamageDelay;

        private Vector3 _startScale;
        private Tween _scaleTween;

        private void Awake()
        {
            Health = GetComponent<Health>();
            _collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            _startScale = transform.localScale;
            _verticalPosOffset = transform.position.y;
            if (_scaleTween == null)
            {
                _scaleTween = transform.StartGrowShrinkLoopTween(_startScale * _stats.AnimationScaleMultiplier);
            }
            else
            {
                _scaleTween.Restart();
            }
        }

        private void OnDisable()
        {
            transform.localScale = _startScale;
            transform.position = new Vector3(0, _verticalPosOffset, 0);
            _scaleTween.Pause();

            Player.PlayerManager.Instance.LevelController.AddExp(_stats.ExpForKill);
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
                             && collider.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(_stats.Damage);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            int numberOfSegments = 16;
            new Debug().DrawCircle(transform.position, _collisionRadius, numberOfSegments, Color.yellow);
        }

        public void ApplyKnockBack(Vector3 locationAfterKnockBack, float timeToArriveAtLocation)
        {
            transform.DOMove(locationAfterKnockBack, timeToArriveAtLocation);
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
            Cell currentCell = WorldPosToCellConverter.GetCellFromGridByWorldPos(grid, transform.position);
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
