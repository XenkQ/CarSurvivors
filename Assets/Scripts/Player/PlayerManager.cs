using Assets.Scripts.HealthSystem;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Health))]
    public sealed class PlayerManager : MonoBehaviour, IDamageable
    {
        public static PlayerManager Instance { get; private set; }

        public Health Health { get; private set; }

        private readonly float _startDelayBetweenTakingDamage = 0.2f;
        private float _currentDelayBetweenTakingDamage;
        private bool _canTakeDamage = true;

        private PlayerManager()
        { }

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
            _currentDelayBetweenTakingDamage = _startDelayBetweenTakingDamage;
            Health.OnHealthDecreased += Health_OnHealthDecreased;
            Health.OnNoHealth += Health_OnNoHealth;
        }

        private void Update()
        {
            if (!_canTakeDamage && _currentDelayBetweenTakingDamage < 0)
            {
                _canTakeDamage = true;
            }
            else if (_currentDelayBetweenTakingDamage > 0)
            {
                _currentDelayBetweenTakingDamage -= Time.deltaTime;
            }
        }

        public void TakeDamage(float damage)
        {
            if (_canTakeDamage)
            {
                Health.DecreaseHealth(damage);
                _canTakeDamage = false;
                _currentDelayBetweenTakingDamage = _startDelayBetweenTakingDamage;
            }
        }

        private void Health_OnHealthDecreased(object sender, System.EventArgs e)
        {
            Debug.Log("DAMAGED");
        }

        private void Health_OnNoHealth(object sender, System.EventArgs e)
        {
            Debug.Log("You are dead");
        }
    }
}
