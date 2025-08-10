using Assets.Scripts.Audio;
using Assets.Scripts.Storage;

namespace Assets.Scripts.Settings
{
    public class AudioVolumeSetting : IAppStorageValue<float>, ISettingLoader
    {
        private readonly IAudioMixersManager _audioMixersManager;

        public float DefaultValue => -6.02f;

        public AudioVolumeSetting(IAudioMixersManager audioMixersManager)
        {
            _audioMixersManager = audioMixersManager;
        }

        public string GetKey()
        {
            return "Volume";
        }

        public float GetValueOrStoredDefault()
        {
            if (AppStorage.TryGet(GetKey(), out float value))
            {
                return value;
            }

            return DefaultValue;
        }

        public void SaveValue(float value)
        {
            AppStorage.Set(GetKey(), value);
        }

        public void Load()
        {
            _audioMixersManager.SetMixerVolume(volume: GetValueOrStoredDefault());
        }
    }
}