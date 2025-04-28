using Grid;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Collider))]
public class Enemy : MonoBehaviour, IDamagable, IKillable
{
    [SerializeField] private float _movementSpeed;
    [SerializeField][Range(0, 1f)] private float _wandererJitter;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private float _pushFromCollisionPower = 1f;

    [SerializeField] private float _damage = 1f;

    [SerializeField] private LayerMask _collisionLayerMask;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _playerLayer;

    [SerializeField] private int _difficulty = 1;

    private Collider _collider;
    private Health _health;
    private float _verticalPosOffset;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _health.onNoHealth.AddListener(Kill);
        _verticalPosOffset = transform.position.y;
    }

    private void OnDisable()
    {
        _health.onNoHealth.RemoveListener(Kill);
    }

    private void Update()
    {
        MoveOnGrid();
    }

    private void FixedUpdate()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _collisionRadius, Vector3.up, Mathf.Infinity, _collisionLayerMask);
        foreach (var hit in hits)
        {
            Collider collider = hit.collider;
            if (hit.collider != null && hit.collider != _collider)
            {
                if ((1 << collider.gameObject.layer) == _enemyLayer.value)
                {
                    PreventInterectingWithColliderByPush(collider);
                }
                else if ((1 << collider.gameObject.layer) == _playerLayer.value)
                {
                    var damagable = collider.gameObject.GetComponent<IDamagable>();
                    damagable.TakeDamage(_damage);
                }
            }
        }
    }

    private void PreventInterectingWithColliderByPush(Collider collider)
    {
        Vector3 otherEnemyDirection = (collider.transform.position - transform.position).normalized;
        Vector3 pushDestination = transform.position + (-otherEnemyDirection * _pushFromCollisionPower);
        pushDestination.y = _verticalPosOffset;
        transform.position = Vector3.Lerp(transform.position, pushDestination, _movementSpeed * Time.deltaTime);
    }

    private void MoveOnGrid()
    {
        Grid.Grid grid = GridController.Instance.WorldGrid;
        Cell currentCell = grid.GetCellFromWorldPos(transform.position);
        if (currentCell != null && currentCell.BestDirection != null)
        {
            Vector2Int gridDirection = currentCell.BestDirection.Vector;
            Vector3 moveDirection = new Vector3(gridDirection.x, 0, gridDirection.y);
            transform.position += _movementSpeed * Time.deltaTime * moveDirection;
        }
    }

    public void TakeDamage(float damage)
    {
        _health.DecreaseHealth(damage);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        int numberOfSegments = 16;
        DebugDraw.DrawCircle(transform.position, _collisionRadius, numberOfSegments, Color.yellow);
    }
}