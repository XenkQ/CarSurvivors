using System;
using UnityEngine;

namespace Assets.Scripts.HealthSystem
{
    public interface IRegenativeHealth : IHealth
    {
        public float MaxRegenerationAmount { get; }
        public float CurrentRegenerationAmount { get; }
        public float CurrentRegenerationDelay { get; }
        public float StartRegenerationDelay { get; }
    }

    [Serializable]
    public class RegenativeHealth : Health, IRegenativeHealth
    {
        [field: SerializeField] public float MaxRegenerationAmount { get; private set; }
        [field: SerializeField] public float StartRegenerationDelay { get; private set; }

        public float CurrentRegenerationAmount { get; private set; }
        public float CurrentRegenerationDelay { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();

            CurrentRegenerationAmount = MaxRegenerationAmount;
            CurrentRegenerationDelay = StartRegenerationDelay;
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
    }
}
