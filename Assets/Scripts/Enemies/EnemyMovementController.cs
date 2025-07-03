using Assets.Scripts.GridSystem;
using Assets.Scripts.StatusAffectables;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovementController : MonoBehaviour, IMovementController
    {
        private const float MOVING_TO_POSITION_ACCURACY = 0.02f;

        private Enemy _enemy;

        private float _verticalPosOffset;

        private IStunable _stunableSelf;
        private bool _isStunable;

        private bool _isMovingToPositionUnrelatedToGrid;
        private Vector3 _currentMovementPositionUnrelatedToGrid;
        private Vector3 _lastPos;

        private Tween _movementUnrelatedToSpeedTween;

        private Vector3 _externalSeparation = Vector3.zero;

        private float _movementDelayAfterAttack = 0.2f;
        private float _currentMovementDelayAfterAttack;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();

            if (gameObject.TryGetComponent(out IStunable stunable))
            {
                _stunableSelf = stunable;
            }
        }

        private void OnEnable()
        {
            _enemy.EnemyAnimator.OnAttackAnimationEnd += EnemyAnimator_OnAttackAnimationEnd;

            _verticalPosOffset = transform.position.y;

            _currentMovementDelayAfterAttack = 0;
        }

        private void OnDisable()
        {
            _enemy.EnemyAnimator.OnAttackAnimationEnd -= EnemyAnimator_OnAttackAnimationEnd;

            transform.position = new Vector3(0, _verticalPosOffset, 0);
        }

        private void Start()
        {
            if (_stunableSelf != null)
            {
                _isStunable = true;
            }
        }

        private void Update()
        {
            bool isStunned = _isStunable && _stunableSelf.StunController.IsStunned;

            bool canMoveOnGrid = _movementUnrelatedToSpeedTween is null
                && !isStunned
                && !_enemy.EnemyAnimator.IsPlayingAttackAnimation
                && _currentMovementDelayAfterAttack <= 0;

            if (canMoveOnGrid)
            {
                _lastPos = transform.position;

                Vector3? movement;
                if (_isMovingToPositionUnrelatedToGrid)
                {
                    movement = MoveToPosition(_currentMovementPositionUnrelatedToGrid);
                }
                else
                {
                    movement = MoveOnFlowFieldGrid();
                }

                if (movement.HasValue)
                {
                    RotateTowardsMovementDirection(movement.Value);
                }
            }
            else if (_currentMovementDelayAfterAttack > 0)
            {
                _currentMovementDelayAfterAttack -= Time.deltaTime;
            }
        }

        public void SetSeparationVector(Vector3 separation)
        {
            _externalSeparation = separation;
        }

        public float GetCurrentMovementSpeed()
        {
            return Vector3.Distance(transform.position, _lastPos) / Time.deltaTime;
        }

        public Tween MoveToPositionInTimeIgnoringSpeed(Vector3 pos, float time)
        {
            if (_movementUnrelatedToSpeedTween != null)
            {
                _movementUnrelatedToSpeedTween.Kill();
            }

            _isMovingToPositionUnrelatedToGrid = true;

            return _movementUnrelatedToSpeedTween = transform
                .DOMove(pos, time)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    _isMovingToPositionUnrelatedToGrid = false;
                    _movementUnrelatedToSpeedTween = null;
                    _externalSeparation = Vector3.zero;
                });
        }

        private void EnemyAnimator_OnAttackAnimationEnd(object sender, System.EventArgs e)
        {
            _currentMovementDelayAfterAttack = _movementDelayAfterAttack;
        }

        private Vector3? MoveToPosition(Vector3 pos)
        {
            bool isOnPosition = Vector3.Distance(transform.position, pos) <= MOVING_TO_POSITION_ACCURACY;
            if (_isMovingToPositionUnrelatedToGrid && !isOnPosition)
            {
                return Move(pos);
            }
            else if (isOnPosition)
            {
                _isMovingToPositionUnrelatedToGrid = false;
                return null;
            }
            else
            {
                _currentMovementPositionUnrelatedToGrid = pos;
                _isMovingToPositionUnrelatedToGrid = true;
                return Move(pos);
            }
        }

        private Vector3 Move(Vector3 pos)
        {
            Vector3 movement = Vector3.Lerp(transform.position, pos, _enemy.Config.MovementSpeed * Time.deltaTime);
            transform.position = movement;
            return movement;
        }

        private void RotateTowardsMovementDirection(Vector3 movement)
        {
            if (GetCurrentMovementSpeed() > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    _enemy.Config.RotationSpeed * Time.deltaTime
                );
            }
        }

        private Vector3 MoveOnFlowFieldGrid()
        {
            Vector3 gridDir = GetMoveDirectionBasedOnCurrentCell();
            Vector3 moveDir = (gridDir + _externalSeparation).normalized;
            Vector3 movement = _enemy.Config.MovementSpeed * Time.deltaTime * moveDir;
            transform.position += movement;

            _externalSeparation = Vector3.zero;

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
    }
}
