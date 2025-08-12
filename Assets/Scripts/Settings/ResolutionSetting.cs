using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Storage;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    public class ResolutionSetting : IAppStorageValue<Resolution>, ISettingLoader
    {
        public Resolution DefaultValue => ScreenResolutionsHelper
            .GetAvailableResolutions()
            .FirstOrDefault();

        private readonly FullScreenSetting _fullScreenSetting;

        public ResolutionSetting(FullScreenSetting fullScreenSetting)
        {
            _fullScreenSetting = fullScreenSetting;
        }

        public string GetKey()
        {
            return "Resolution";
        }

        public Resolution GetValueOrStoredDefault()
        {
            if (AppStorage.TryGetValue(GetKey(), out Resolution storedResolution))
            {
                return storedResolution;
            }

            return DefaultValue;
        }

        public void SaveValue(Resolution value)
        {
            AppStorage.SetValue(GetKey(), value);
        }

        public void Load()
        {
            var storedValue = GetValueOrStoredDefault();

            Resolution resolution = ScreenResolutionsHelper
                .GetAvailableResolutions()
                .FirstOrDefault(r => r.Equals(storedValue));

            if (resolution.Equals(default(Resolution)))
            {
                resolution = DefaultValue;
            }

            Screen.SetResolution(
                resolution.width,
                resolution.height,
                _fullScreenSetting.GetValueOrStoredDefault()
            );
        }
    }
}