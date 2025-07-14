using Assets.Scripts.Initializers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.DamagePopups
{
    public struct DamagePopupConfig
    {
        public float Damage;
        public float FontSize;
        public Color Color;

        public DamagePopupConfig(float damage, float fontSize, Color color)
        {
            Damage = damage;
            FontSize = fontSize;
            Color = color;
        }
    }

    public class DamagePopup : MonoBehaviour, IInitializable<DamagePopupConfig>
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        private bool _isInitialized;

        public void Initialize(DamagePopupConfig config)
        {
            _textMeshPro.text = config.Damage.ToString();
            _textMeshPro.color = config.Color;
            _textMeshPro.fontSize = config.FontSize;
            _isInitialized = true;
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }
    }
}
