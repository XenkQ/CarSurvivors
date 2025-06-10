using Assets.Scripts.GridSystem;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private EnemyConfigSO _config;
        private float _verticalPosOffset;
        private bool _isMovingToPositionUnrelatedToGrid;
        private IStunable _stunableSelf;
        private bool _isStunable;

        private void Awake()
        {
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
            if (!isStunned && !_isMovingToPositionUnrelatedToGrid)
            {
                Vector3 movement = MoveOnGrid();
                RotateTowardsMovementDirection(movement);
            }
            else if (!isStunned && _isMovingToPositionUnrelatedToGrid)
            {
                MoveToPosition();
            }
        }

        public void MoveToPosition()
        {
        }

        private void RotateTowardsMovementDirection(Vector3 movement)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _config.RotationSpeed * Time.deltaTime);
        }

        private Vector3 MoveOnGrid()
        {
            Vector3 gridDir = GetMoveDirectionBasedOnCurrentCell();
            Vector3 movement = _config.MovementSpeed * Time.deltaTime * gridDir;
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
