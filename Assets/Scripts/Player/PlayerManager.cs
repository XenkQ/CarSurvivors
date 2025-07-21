using Assets.Scripts.Car;
using Assets.Scripts.HealthSystem;
using Assets.Scripts.LevelSystem;
using Assets.Scripts.Skills;
using Assets.Scripts.StatusAffectables;
using UnityEngine;
using VFX;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(RegenativeHealth), typeof(LevelController))]
    public sealed class PlayerManager : MonoBehaviour, IHealthy, IDamageable
    {
        public static PlayerManager Instance { get; private set; }

        public IHealth Health { get; private set; }
        public ILevelController LevelController { get; private set; }
        public ISkillsRegistry SkillsRegistry { get; private set; }
        public ICarController CarController { get; private set; }

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
            SkillsRegistry = GetComponentInChildren<ISkillsRegistry>();
            CarController = GetComponent<ICarController>();
        }

        private void Start()
        {
            _currentDelayBetweenTakingDamage = _startDelayBetweenTakingDamage;
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

        public void InstantKill()
        {
            Health.DecreaseHealth(Health.MaxHealth);
        }
    }
}
