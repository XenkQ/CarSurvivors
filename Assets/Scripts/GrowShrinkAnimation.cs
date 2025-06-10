using Assets.Scripts.Extensions;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class GrowShrinkAnimation : MonoBehaviour
    {
        private Vector3 _startScale;
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
                _scaleTween = transform.StartGrowShrinkLoopTween(_startScale * _stats.AnimationScaleMultiplier);
            }
            else
            {
                _scaleTween.Restart();
            }
        }
    }
}
