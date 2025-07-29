using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Assets.Scripts.Storage;

namespace Assets.Scripts.UI.Settings
{
    public class VolumeOption : MonoBehaviour, ISettingsOption<float>
    {
        private const string PLAYER_PREFS_KEY = "Volume";

        [SerializeField] private TextMeshProUGUI _volumeText;
        [SerializeField] private Slider _slider;
        [SerializeField] private AudioMixer _audioMixer;

        private float _volume;

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(OnValueChanged);
            LoadValue();
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnValueChanged);
            SaveValue();
        }

        public void LoadValue()
        {
            _volume = AppStorage.Get<float>(PLAYER_PREFS_KEY);
            SetSliderValueBasedOnVolumeValue();
            UpdateText();
        }

        public void SaveValue()
        {
            AppStorage.Set(PLAYER_PREFS_KEY, _volume);
        }

        public void OnValueChanged(float value)
        {
            UpdateText();
            UpdateVolumeValue();
            _audioMixer.SetFloat(PLAYER_PREFS_KEY, _volume);
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
