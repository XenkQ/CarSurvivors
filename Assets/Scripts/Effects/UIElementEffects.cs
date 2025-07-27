using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class UIElementEffects : MonoBehaviour
    {
        [SerializeField] private float _growScaleMultiplier = 1.2f;
        [SerializeField] private float _growAnimationDuration = 0.2f;
        [SerializeField] private float _shrinkAnimatioDuration = 0.1f;

        public void PlayGrowAnimation(RectTransform rect)
        {
            DOTween.Kill(rect);
            rect.DOScale(Vector3.one * _growScaleMultiplier, _growAnimationDuration)
                .SetUpdate(true);
        }

        public void PlayShrinkAnimation(RectTransform rect)
        {
            DOTween.Kill(rect);
            rect.DOScale(Vector3.one, _shrinkAnimatioDuration)
                .SetUpdate(true);
        }
    }
}
