using System;
using Assets.Scripts.Storage;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Settings
{
    public class AudioVolumeSetting : IAppStorageValue<float>, ISettingLoader
    {
        [SerializeField] private AudioMixer _audioMixer;

        public string GetKey()
        {
            return "AudioVolume";
        }

        public float GetValue()
        {
            return AppStorage.Get<float>(GetKey());
        }

        public void SaveValue(float value)
        {
            AppStorage.Set(GetKey(), value);
        }

        public void Load()
        {
            _audioMixer.SetFloat(GetKey(), GetValue());
        }
    }
}