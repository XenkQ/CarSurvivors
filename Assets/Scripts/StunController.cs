using System;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IStunable
    {
        public IStunController StunController { get; }
    }

    public interface IStunController
    {
        public bool IsStunned { get; }

        public event EventHandler OnStunEnd;

        public event EventHandler OnStunStart;

        public void ApplyStun();
    }

    public class StunController : MonoBehaviour, IStunController
    {
        [SerializeField] private float _stunDuration = 0.5f;

        public bool IsStunned { get; private set; }

        public event EventHandler OnStunEnd;

        public event EventHandler OnStunStart;

        private float _stunTimer;

        private void Update()
        {
            if (IsStunned)
            {
                _stunTimer -= Time.deltaTime;
                if (_stunTimer <= 0f)
                {
                    IsStunned = false;
                    OnStunEnd?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void ApplyStun()
        {
            if (!IsStunned)
            {
                IsStunned = true;
                _stunTimer = _stunDuration;
                OnStunStart?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
