using Assets.Scripts.Initializers;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.DamageNumbers
{
    public struct DamageNumberConfig
    {
        public float Damage;
        public DamageNumberApearance DamagePopupApearance;

        public DamageNumberConfig(float damage, DamageNumberApearance damagePopupApearance)
        {
            Damage = damage;
            DamagePopupApearance = damagePopupApearance;
        }
    }

    public class DamageNumber : MonoBehaviour, IInitializable<DamageNumberConfig>
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        private bool _isInitialized;
        private const float RESIZING_ANIMATION_SPEED = 0.6f;
        private DamageNumberConfig _config;

        public event EventHandler OnLifeEnd;

        public void Initialize(DamageNumberConfig config)
        {
            _config = config;

            SetTextApearance(config);

            var (fontSize, growFontSizeAnimationScaleMultiplier, _) = _config.DamagePopupApearance;
            AnimateFontGrowing(fontSize * growFontSizeAnimationScaleMultiplier)
                .OnComplete(() =>
                {
                    AnimateFontDisapearing()
                        .OnComplete(() => OnLifeEnd?.Invoke(this, EventArgs.Empty));
                });

            _isInitialized = true;
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }

        private void SetTextApearance(DamageNumberConfig config)
        {
            var (fontSize, growFontSizeAnimationScaleMultiplier, color) = config.DamagePopupApearance;
            _textMeshPro.text = config.Damage.ToString();
            _textMeshPro.color = color;
            _textMeshPro.fontSize = fontSize;
        }

        private Tween AnimateFontGrowing(float fontSizeDestination)
        {
            return DOTween.To(
                () => _textMeshPro.fontSize,
                (value) => _textMeshPro.fontSize = value,
                fontSizeDestination,
                RESIZING_ANIMATION_SPEED
            )
            .SetEase(Ease.InOutSine);
        }

        private Tween AnimateFontDisapearing()
        {
            return DOTween.To(
                () => _textMeshPro.fontSize,
                (value) => _textMeshPro.fontSize = value,
                0,
                RESIZING_ANIMATION_SPEED
            )
            .SetEase(Ease.InOutSine);
        }
    }
}
