using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Reflex.Attributes;
using Assets.Scripts.Settings;

namespace Assets.Scripts.UI.Settings
{
    public class AudioVolumeOption : MonoBehaviour
    {
        [Inject] private readonly AudioVolumeSetting _audioVolumeSetting;

        [SerializeField] private TextMeshProUGUI _volumeText;
        [SerializeField] private Slider _slider;

        private float _volume = 50f;

        private void OnEnable()
        {
            LoadComponent();
            _slider.onValueChanged.AddListener(PerformValueChange);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(PerformValueChange);
        }

        public void PerformValueChange(float value)
        {
            _audioVolumeSetting.SaveValue(_volume);
            _audioVolumeSetting.Load();

            UpdateText();
            UpdateVolumeValue();
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

        private void LoadComponent()
        {
            _volume = _audioVolumeSetting.GetValue();
            PerformValueChange(_volume);
            SetSliderValueBasedOnVolumeValue();
        }
    }
}