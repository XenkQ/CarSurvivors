using UnityEngine;
using DG.Tweening;

namespace Player.Skills
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private float _range = 2f;
        [SerializeField][Range(-180f, 180f)] private float _minAngle;
        [SerializeField][Range(-180f, 180f)] private float _maxAngle;
        [SerializeField] private float _rotationDuration = 0.3f;
        private Tween _rotateTween;

        public void OnEnable()
        {
            transform.rotation = Quaternion.Euler(0, _minAngle, 0);

            if (_rotateTween == null)
            {
                _rotateTween = transform.DORotate(new Vector3(0, _minAngle, 0), _rotationDuration).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _rotateTween.Restart();
            }
        }

        public void OnDisable()
        {
            _rotateTween.Pause();
        }
    }
}
