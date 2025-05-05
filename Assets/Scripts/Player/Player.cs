using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance { get; private set; }

    public Health Health { get; private set; }

    private readonly float _startDelayBetweenTakingDamage = 0.2f;
    private float _currentDelayBetweenTakingDamage;
    private bool _canTakeDamage = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Health = GetComponent<Health>();
    }

    private void Start()
    {
        _currentDelayBetweenTakingDamage = _startDelayBetweenTakingDamage;
        Health.OnHealthDecreased += DamageEffect_OnHealthDecreased;
        Health.OnNoHealth += DeadEffect_OnNoHealth;
    }

    private void Update()
    {
        if (!_canTakeDamage && _currentDelayBetweenTakingDamage < 0)
        {
            _canTakeDamage = true;
        }
        else if (_currentDelayBetweenTakingDamage > 0)
        {
            _currentDelayBetweenTakingDamage -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (_canTakeDamage)
        {
            Health.DecreaseHealth(damage);
            _canTakeDamage = false;
            _currentDelayBetweenTakingDamage = _startDelayBetweenTakingDamage;
        }
    }

    private void DamageEffect_OnHealthDecreased(object sender, System.EventArgs e)
    {
        Debug.Log("DAMAGED");
    }

    private void DeadEffect_OnNoHealth(object sender, System.EventArgs e)
    {
        Debug.Log("You are dead");
    }
}
