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
        public float CurrentHealth { get; }
        public float MaxHealth { get; set; }

        public event EventHandler OnHealthChange;

        public event EventHandler OnHealthDecreased;

        public event EventHandler OnHealthIncreased;

        public event EventHandler OnNoHealth;

        public void DecreaseHealth(float value);

        public void IncreaseHealth(float value);

        public bool IsAlive();
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

        private bool _isAlive;

        protected virtual void OnEnable()
        {
            OnHealthDecreased += InvokeOnHealthChange;
            OnHealthIncreased += InvokeOnHealthChange;
            OnNoHealth += InvokeOnHealthChange;

            _isAlive = true;
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
            if (!_isAlive)
            {
                return;
            }

            if (CurrentHealth > value)
            {
                CurrentHealth -= value;
                OnHealthDecreased?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                CurrentHealth = 0;
                _isAlive = false;
                OnNoHealth?.Invoke(this, EventArgs.Empty);
            }
        }

        public void IncreaseHealth(float value)
        {
            if (!_isAlive)
            {
                return;
            }

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

        public bool IsAlive()
        {
            return _isAlive;
        }

        private void InvokeOnHealthChange(object sender, EventArgs e)
        {
            OnHealthChange?.Invoke(sender, e);
        }
    }
}
