using UnityEngine;
using DG.Tweening;

namespace Player.Skills
{
    public class MinigunTurret : MonoBehaviour
    {
        [field: SerializeField] public Transform GunTip { get; private set; }
        [SerializeField] private Transform _visual;
        [SerializeField] private float _range = 2f;
        [SerializeField][Range(0, 180f)] private float _radiusAngle;
        [SerializeField] private bool _inverseRotation;
        [SerializeField] private float _rotationDuration = 0.3f;
        private Tween _rotateTween;

        private Quaternion _startRotation;

        public void OnEnable()
        {
            _visual.localRotation = Quaternion.Euler(0, (_inverseRotation ? _radiusAngle : -_radiusAngle) * 0.5f, 0);

            if (_rotateTween == null)
            {
                _rotateTween = _visual.DOLocalRotate(new Vector3(0, (_inverseRotation ? -_radiusAngle : _radiusAngle) * 0.5f, 0), _rotationDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _rotateTween.Restart();
            }
        }

        public void OnDisable()
        {
            transform.rotation = _startRotation;
            _rotateTween.Pause();
        }
    }
}
