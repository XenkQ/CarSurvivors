using Assets.Scripts.HealthSystem;
using Assets.Scripts.LevelSystem;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(RegenativeHealth), typeof(LevelController))]
    public sealed class PlayerManager : MonoBehaviour, IHealthy, IDamageable
    {
        public static PlayerManager Instance { get; private set; }

        public IHealth Health { get; private set; }
        public ILevelController LevelController { get; private set; }

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

            Health = GetComponent<IHealth>();
            LevelController = GetComponent<ILevelController>();
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
