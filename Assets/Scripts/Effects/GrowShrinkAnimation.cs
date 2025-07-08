using Assets.Scripts.Extensions;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class GrowShrinkAnimation : MonoBehaviour
    {
        [SerializeField] private float _animationScaleMultiplier = 1f;
        [SerializeField] private float _iterationDuration = 1f;
        private Vector3 _startScale = Vector3.one;
        private Tween _scaleTween;

        private void OnEnable()
        {
            _startScale = transform.localScale;
            AnimateGrowingShrinking();
        }

        private void OnDisable()
        {
            transform.localScale = _startScale;
            _scaleTween.Pause();
        }

        private void AnimateGrowingShrinking()
        {
            if (_scaleTween == null)
            {
                _scaleTween = transform.StartGrowShrinkLoopTween(_startScale * _animationScaleMultiplier, _iterationDuration);
            }
            else
            {
                _scaleTween.Restart();
            }
        }
    }
}
