using Assets.Scripts.Initializers;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.DamagePopups
{
    [Serializable]
    public struct DamagePopupApearance
    {
        public float FontSize;
        public float GrowFontSizeAnimationScaleMultiplier;
        public Color Color;

        public DamagePopupApearance(float fontSize, float growFontSizeAnimationScaleMultiplier, Color color)
        {
            FontSize = fontSize;
            GrowFontSizeAnimationScaleMultiplier = growFontSizeAnimationScaleMultiplier;
            Color = color;
        }

        public void Deconstruct(out float fontSize, out float growFontSizeAnimationScaleMultiplier, out Color color)
        {
            growFontSizeAnimationScaleMultiplier = GrowFontSizeAnimationScaleMultiplier;
            fontSize = FontSize;
            color = Color;
        }
    }

    public struct DamagePopupConfig
    {
        public float Damage;
        public DamagePopupApearance DamagePopupApearance;

        public DamagePopupConfig(float damage, DamagePopupApearance damagePopupApearance)
        {
            Damage = damage;
            DamagePopupApearance = damagePopupApearance;
        }
    }

    public class DamagePopup : MonoBehaviour, IInitializable<DamagePopupConfig>
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        private bool _isInitialized;
        private const float RESIZING_ANIMATION_SPEED = 0.6f;
        private DamagePopupConfig _config;

        public event EventHandler OnLifeEnd;

        public void Initialize(DamagePopupConfig config)
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

        private void SetTextApearance(DamagePopupConfig config)
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
