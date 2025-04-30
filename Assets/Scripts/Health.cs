using System;
using UnityEngine;

[Serializable]
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxRegenerationAmount;
    [SerializeField] private float startRegenerationDelay;
    private float currentHealth;
    private float currentRegenerationAmount;
    private float currentRegenerationDelay;

    public float Amount => currentHealth;
    public float RegenerationAmount => currentRegenerationAmount;
    public float RegenerationDelay => currentRegenerationDelay;

    public event EventHandler OnNoHealth;

    public event EventHandler OnHealthDecreased;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        currentRegenerationAmount = maxRegenerationAmount;
        currentRegenerationDelay = startRegenerationDelay;
    }

    private void Update()
    {
        if (currentRegenerationDelay > 0)
        {
            currentRegenerationDelay -= Time.deltaTime;
        }
        else
        {
            IncreaseHealth(maxRegenerationAmount);
        }
    }

    public void DecreaseHealth(float value)
    {
        if (currentHealth > value)
        {
            currentHealth -= currentHealth;
            OnHealthDecreased?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnNoHealth?.Invoke(this, EventArgs.Empty);
        }
    }

    public void IncreaseHealth(float value)
    {
        if (currentHealth + value < maxHealth)
        {
            currentHealth += value;
        }
        else
        {
            currentHealth = maxHealth;
        }
    }
}
