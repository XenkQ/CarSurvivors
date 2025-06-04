using UnityEngine;
using Assets.Scripts.Extensions;
using Assets.ScriptableObjects;

namespace Assets.Scripts.Skills.PlayerSkills.Minigun
{
    public class MinigunTurret : MonoBehaviour, IInitializableWithScriptableConfig<TurretConfigSO>
    {
        [field: SerializeField] public Transform GunTip { get; private set; }
        [SerializeField] private Transform _visual;
        [SerializeField] private bool _inverseRotation;
        private TurretConfigSO _config;

        public void Initialize(TurretConfigSO config)
        {
            _config = config;
            _visual.localRotation = Quaternion.Euler(0, (_inverseRotation ? _config.RotationAngle : -_config.RotationAngle) * 0.5f, 0);
            _visual.StartYAngleLocalRotationLoopTween(_config.RotationAngle, _config.RotationDuration, _inverseRotation);
        }

        public bool IsInitialized()
        {
            return gameObject.activeSelf;
        }
    }
}
