using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class ScaleUpAnimationOnEnable : MonoBehaviour
    {
        [SerializeField] private Vector3 _startScale = Vector3.one;
        [SerializeField] private Vector3 _endScale = Vector3.one;
        [SerializeField] private float _scalingDuration = 0.3f;
        private RectTransform _rectTransform;

        private void Awake()
        {
            if (TryGetComponent(out RectTransform rectTransform))
            {
                _rectTransform = rectTransform;
            }
        }

        private void OnEnable()
        {
            if (_rectTransform is null)
            {
                transform.localScale = _startScale;
                transform
                    .DOScale(_endScale, _scalingDuration)
                    .SetUpdate(true);
            }
            else
            {
                _rectTransform.localScale = _startScale;
                _rectTransform
                    .DOScale(_endScale, _scalingDuration)
                    .SetUpdate(true);
            }
        }
    }
}
