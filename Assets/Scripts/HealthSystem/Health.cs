using System;
using UnityEngine;

namespace Assets.Scripts.HealthSystem
{
    [Serializable]
    public class Health : MonoBehaviour
    {
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float MaxRegenerationAmount { get; private set; }
        [field: SerializeField] public float StartRegenerationDelay { get; private set; }

        public float CurrentHealth { get; private set; }
        public float CurrentRegenerationAmount { get; private set; }
        public float CurrentRegenerationDelay { get; private set; }

        public event EventHandler OnNoHealth;

        public event EventHandler OnHealthChange;

        public event EventHandler OnHealthDecreased;

        public event EventHandler OnHealthIncreased;

        private void OnEnable()
        {
            OnHealthDecreased += OnHealthChange;
            OnHealthIncreased += OnHealthChange;
            OnNoHealth += OnHealthChange;

            CurrentHealth = MaxHealth;
            CurrentRegenerationAmount = MaxRegenerationAmount;
            CurrentRegenerationDelay = StartRegenerationDelay;
        }

        private void OnDisable()
        {
            OnHealthDecreased -= OnHealthChange;
            OnHealthIncreased -= OnHealthChange;
            OnNoHealth -= OnHealthChange;
        }

        private void Update()
        {
            if (MaxRegenerationAmount > 0 && StartRegenerationDelay > 0)
            {
                RegenerationProcess();
            }
        }

        private void RegenerationProcess()
        {
            if (CurrentRegenerationDelay > 0)
            {
                CurrentRegenerationDelay -= Time.deltaTime;
            }
            else
            {
                if (CurrentHealth > 0)
                {
                    IncreaseHealth(MaxRegenerationAmount);
                    CurrentRegenerationDelay = StartRegenerationDelay;
                }
            }
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
    }
}
