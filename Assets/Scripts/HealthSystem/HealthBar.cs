using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.HealthSystem
{
    [RequireComponent(typeof(Slider))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Health _health;
        [SerializeField] private Image _fillImage;
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(Display), 1f, 1f);
        }

        private void Display()
        {
            Debug.Log("IS PLAYER NOT NULL: " + (_health != null) + " " + _health.CurrentHealth);
        }

        private void OnEnable()
        {
            _slider.maxValue = _health.MaxHealth;
            _slider.value = _health.MaxHealth;
            _health.OnHealthChange += UpdateSlider_OnHealthChange;
        }

        private void OnDisable()
        {
            _health.OnHealthChange -= UpdateSlider_OnHealthChange;
        }

        private void UpdateSlider_OnHealthChange(object sender, System.EventArgs e)
        {
            _fillImage.color = _gradient.Evaluate(_health.CurrentHealth / _health.MaxHealth);
            _slider.value = _health.CurrentHealth;
        }
    }
}
