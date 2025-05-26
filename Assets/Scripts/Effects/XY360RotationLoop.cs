using Assets.Scripts.Extensions;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class XY360RotationLoop : MonoBehaviour
    {
        [SerializeField] private float _tweenIterationTime = 2.5f;
        private Vector3 _maxTweenRotation = new Vector3(360, 360, 0);
        private Tween _rotationTween;

        private void OnEnable()
        {
            if (_rotationTween == null)
            {
                _rotationTween = transform.StartXY360RotateLoopTween(_maxTweenRotation, _tweenIterationTime);
            }
            else
            {
                _rotationTween.Restart();
            }
        }

        private void OnDisable()
        {
            if (_rotationTween != null)
            {
                _rotationTween.Pause();
            }
        }
    }
}
