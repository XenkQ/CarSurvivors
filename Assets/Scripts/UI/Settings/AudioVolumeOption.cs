using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
using Assets.Scripts.CustomEventArgs;

namespace Assets.Scripts.UI.Settings
{
    public class AudioVolumeOption : MonoBehaviour, ISettingsOption<float>
    {
        [SerializeField] private TextMeshProUGUI _volumeText;
        [SerializeField] private Slider _slider;
        [SerializeField] private AudioMixer _audioMixer;

        private float _volume;

        public event EventHandler<ValueEventArgs<float>> OnValueChanged;

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(PerformValueChange);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(PerformValueChange);
        }

        public void PerformValueChange(float value)
        {
            UpdateText();
            UpdateVolumeValue();
            OnValueChanged?.Invoke(this, new(_volume));
        }

        public float GetValue()
        {
            return _volume;
        }

        private void UpdateText()
        {
            _volumeText.text = $"{Mathf.Floor(_slider.value * 100)}%";
        }

        private void UpdateVolumeValue()
        {
            _volume = Mathf.Log10(_slider.value) * 20f;
        }

        private void SetSliderValueBasedOnVolumeValue()
        {
            _slider.value = Mathf.Pow(10f, _volume / 20f);
        }
    }
}