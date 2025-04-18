using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float _movementSpeed;
    private Health _health;
    private Transform _player;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        _health.onNoHealth.AddListener(Die);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(_player.position.x, 0, _player.transform.position.z),
            _movementSpeed * Time.deltaTime
        );
    }

    public void TakeDamage(float damage)
    {
        _health.DecreaseHealth(damage);
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}