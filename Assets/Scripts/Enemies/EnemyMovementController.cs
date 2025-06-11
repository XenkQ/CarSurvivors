using Assets.Scripts.GridSystem;
using Assets.Scripts.StatusAffectables;
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
            _verticalPosOffset = transform.position.y;
        }

        private void OnDisable()
        {
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
            if (!isStunned)
            {
                _lastPos = transform.position;
                Vector3? movement;
                if (_isMovingToPositionUnrelatedToGrid)
                {
                    movement = MoveToPosition(_currentMovementPositionUnrelatedToGrid);
                    if (movement.HasValue)
                    {
                        RotateTowardsMovementDirection(movement.Value);
                    }
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
        }

        public float GetCurrentMovementSpeed()
        {
            return Vector3.Distance(transform.position, _lastPos) / Time.deltaTime;
        }

        public Vector3? MoveToPosition(Vector3 pos)
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
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _enemy.Config.RotationSpeed * Time.deltaTime);
        }

        private Vector3 MoveOnFlowFieldGrid()
        {
            Vector3 gridDir = GetMoveDirectionBasedOnCurrentCell();
            Vector3 movement = _enemy.Config.MovementSpeed * Time.deltaTime * gridDir;
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
    }
}
