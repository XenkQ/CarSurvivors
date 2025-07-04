using System;
using UnityEngine;

namespace Assets.Scripts.HealthSystem
{
    public interface IHealthy
    {
        public IHealth Health { get; }
    }

    public interface IHealth
    {
        float CurrentHealth { get; }
        float MaxHealth { get; set; }

        event EventHandler OnHealthChange;

        event EventHandler OnHealthDecreased;

        event EventHandler OnHealthIncreased;

        event EventHandler OnNoHealth;

        void DecreaseHealth(float value);

        void IncreaseHealth(float value);
    }

    [Serializable]
    public class Health : MonoBehaviour, IHealth
    {
        [field: SerializeField] public float MaxHealth { get; set; }
        public float CurrentHealth { get; protected set; }

        public event EventHandler OnNoHealth;

        public event EventHandler OnHealthChange;

        public event EventHandler OnHealthDecreased;

        public event EventHandler OnHealthIncreased;

        protected virtual void OnEnable()
        {
            OnHealthDecreased += InvokeOnHealthChange;
            OnHealthIncreased += InvokeOnHealthChange;
            OnNoHealth += InvokeOnHealthChange;

            CurrentHealth = MaxHealth;
        }

        protected virtual void OnDisable()
        {
            OnHealthDecreased -= InvokeOnHealthChange;
            OnHealthIncreased -= InvokeOnHealthChange;
            OnNoHealth -= InvokeOnHealthChange;
        }

        public void DecreaseHealth(float value)
        {
            if (CurrentHealth > value)
            {
                CurrentHealth -= value;
                OnHealthDecreased?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                CurrentHealth = 0;
                OnNoHealth?.Invoke(this, EventArgs.Empty);
            }
        }

        public void IncreaseHealth(float value)
        {
            if (CurrentHealth + value < MaxHealth)
            {
                CurrentHealth += value;
                OnHealthIncreased?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                CurrentHealth = MaxHealth;
            }
        }

        private void InvokeOnHealthChange(object sender, EventArgs e)
        {
            OnHealthChange?.Invoke(sender, e);
        }
    }
}
