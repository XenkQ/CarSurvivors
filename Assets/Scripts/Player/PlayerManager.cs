using Assets.Scripts.Audio;
using Assets.Scripts.Car;
using Assets.Scripts.HealthSystem;
using Assets.Scripts.LevelSystem;
using Assets.Scripts.Skills;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(RegenativeHealth), typeof(LevelController))]
    public sealed class PlayerManager : MonoBehaviour, IHealthy
    {
        public static PlayerManager Instance { get; private set; }

        public IHealth Health { get; private set; }
        public ILevelController LevelController { get; private set; }
        public ISkillsRegistry SkillsRegistry { get; private set; }
        public ICarController CarController { get; private set; }
        public IAudioClipPlayer AudioClipPlayer { get; private set; }

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
            AudioClipPlayer = GetComponentInChildren<IAudioClipPlayer>();
        }
    }
}
