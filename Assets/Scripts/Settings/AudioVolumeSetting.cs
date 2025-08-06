using Assets.Scripts.Audio;
using Assets.Scripts.Storage;

namespace Assets.Scripts.Settings
{
    public class AudioVolumeSetting : IAppStorageValue<float>, ISettingLoader
    {
        private readonly IAudioMixersManager _audioMixersManager;

        public AudioVolumeSetting(IAudioMixersManager audioMixersManager)
        {
            _audioMixersManager = audioMixersManager;
        }

        public string GetKey()
        {
            return "Volume";
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
            _audioMixersManager.SetMixerVolume(volume: GetValue());
        }
    }
}