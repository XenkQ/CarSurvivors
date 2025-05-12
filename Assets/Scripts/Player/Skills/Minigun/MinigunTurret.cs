using UnityEngine;
using DG.Tweening;

namespace Player.Skills
{
    public class MinigunTurret : MonoBehaviour
    {
        [field: SerializeField] public Transform GunTip { get; private set; }
        [SerializeField] private Transform _visual;
        [SerializeField] private TurretStatsSO _stats;
        [SerializeField] private bool _inverseRotation;
        private Tween _rotateTween;

        private Quaternion _startRotation;

        private void OnEnable()
        {
            _visual.localRotation = Quaternion.Euler(0, (_inverseRotation ? _stats.RadiusAngle : -_stats.RadiusAngle) * 0.5f, 0);

            if (_rotateTween == null)
            {
                _rotateTween = _visual
                    .DOLocalRotate(new Vector3(0, (_inverseRotation ? -_stats.RadiusAngle : _stats.RadiusAngle) * 0.5f, 0), _stats.RotationDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _rotateTween.Restart();
            }
        }

        private void OnDisable()
        {
            transform.rotation = _startRotation;
            _rotateTween.Pause();
        }
    }
}
