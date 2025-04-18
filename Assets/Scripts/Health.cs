using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Health : MonoBehaviour
{
    [SerializeField] private float amount;
    [SerializeField] private float regenerationAmount;
    [SerializeField] private float startRegenerationDelay;
    private float regenerationDelay;

    public float Amount => amount;
    public float RegenerationAmount => regenerationAmount;
    public float RegenerationDelay => regenerationDelay;

    public UnityEvent onNoHealth;

    private void OnEnable()
    {
        regenerationDelay = startRegenerationDelay;
    }

    private void Update()
    {
        if (regenerationDelay > 0)
        {
            regenerationDelay -= Time.deltaTime;
        }
        else
        {
            IncreaseHealth(regenerationAmount);
        }
    }

    public void DecreaseHealth(float value)
    {
        if (amount - value > 0)
        {
            amount -= amount;
        }
        else
        {
            onNoHealth?.Invoke();
        }
    }

    public void IncreaseHealth(float value)
    {
        if (amount > 0)
        {
            amount += value;
        }
    }
}