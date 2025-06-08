using UnityEngine;
using Assets.Scripts.Extensions;
using Assets.ScriptableObjects;
using DG.Tweening;

namespace Assets.Scripts.Skills.PlayerSkills.Minigun
{
    public class MinigunTurret : MonoBehaviour, IInitializableWithScriptableConfig<TurretConfigSO>
    {
        [field: SerializeField] public Transform GunTip { get; private set; }
        [SerializeField] private Transform _visual;
        [SerializeField] private bool _inverseRotation;
        private TurretConfigSO _config;
        private Tween _rotationTween;
        private bool _isInitialized;

        public void Initialize(TurretConfigSO config)
        {
            _config = config;
            _isInitialized = true;
            _visual.localRotation = Quaternion.Euler(0, (_inverseRotation ? _config.RotationAngle : -_config.RotationAngle) * 0.5f, 0);
            StartInYAngleRotation();
        }

        public bool IsInitialized() => _isInitialized;

        private void StartInYAngleRotation()
        {
            if (_rotationTween != null)
            {
                _rotationTween.Kill();
            }

            _rotationTween = _visual.StartYAngleLocalRotationLoopTween(_config.RotationAngle, _config.RotationDuration, _inverseRotation);
        }
    }
}
