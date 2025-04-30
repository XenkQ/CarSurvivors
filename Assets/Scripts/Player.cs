using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance { get; private set; }

    public Health Health { get; private set; }

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
        Health.OnHealthDecreased += DamageEffect_OnHealthDecreased;
        Health.OnNoHealth += DeadEffect_OnNoHealth;
    }

    public void TakeDamage(float damage)
    {
        Health.DecreaseHealth(damage);
    }

    private void DamageEffect_OnHealthDecreased(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void DeadEffect_OnNoHealth(object sender, System.EventArgs e)
    {
        Debug.Log("You are dead");
    }
}
