using UnityEngine;

[RequireComponent(typeof(Health), typeof(Collider))]
public class Enemy : MonoBehaviour, IDamagable, IKillable
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private LayerMask _collisionLayerMask;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _pushFromCollisionPower = 1f;
    [SerializeField] private float _damage = 1f;
    private Collider _collider;
    private Health _health;
    private Transform _player;
    private float _verticalPosOffset;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
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
        MoveInPlayerDirection();
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

    private void MoveInPlayerDirection()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(_player.position.x, transform.position.y, _player.transform.position.z),
            _movementSpeed * Time.deltaTime
        );
    }

    public void TakeDamage(float damage)
    {
        _health.DecreaseHealth(damage);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}