using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour, IDamagable, IKillable
{
    private Health _health;

    private void Awake()
    {
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