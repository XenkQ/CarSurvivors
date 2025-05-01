using Grid;
using LayerMasks;
using System;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Collider))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _movementSpeed;
    [SerializeField][Range(0, 1f)] private float _wandererJitter;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private float _pushFromCollisionPower = 1f;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private int _level = 1;

    private Collider _collider;
    private float _verticalPosOffset;

    public Health Health { get; private set; }

    private void Awake()
    {
        Health = GetComponent<Health>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        MoveOnGrid();
    }

    private void FixedUpdate()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _collisionRadius, Vector3.up, Mathf.Infinity, EntityLayers.All);
        foreach (var hit in hits)
        {
            Collider collider = hit.collider;
            if (hit.collider != null && hit.collider != _collider)
            {
                if ((1 << collider.gameObject.layer) == EntityLayers.Enemy)
                {
                    PreventInterectingWithColliderByPush(collider);
                }
                else if ((1 << collider.gameObject.layer) == EntityLayers.Player)
                {
                    collider.gameObject.TryGetComponent(out IDamageable damagable);
                    damagable.TakeDamage(_damage);
                }
            }
        }
    }

    public void SpawnAtCell(Cell cell)
    {
        transform.position = cell.WorldPos;
    }

    public void TakeDamage(float damage)
    {
        throw new NotImplementedException();
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

    private void OnDrawGizmos()
    {
        int numberOfSegments = 16;
        DebugUtilities.DrawCircle(transform.position, _collisionRadius, numberOfSegments, Color.yellow);
    }
}
