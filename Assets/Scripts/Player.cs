using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour, IDamagable, IKillable
{
    private Health _health;
    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }

        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _health.onNoHealth.AddListener(Kill);
    }

    public void TakeDamage(float damage)
    {
        _health.DecreaseHealth(damage);
    }

    public void Kill()
    {
        Debug.Log("You are dead");
    }
}