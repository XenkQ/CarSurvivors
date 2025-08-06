using Assets.Scripts.Storage;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class GraphicSetting : IAppStorageValue<string>, ISettingLoader
    {
        public string GetKey()
        {
            return "GraphicsQuality";
        }

        public string GetValue()
        {
            return AppStorage.Get<string>(GetKey());
        }

        public void SaveValue(string value)
        {
            AppStorage.Set(GetKey(), value);
        }

        public void Load()
        {
            int savedLevel = Array.IndexOf(QualitySettings.names, GetValue());
            SetQualityLevel(savedLevel);
        }

        private void SetQualityLevel(int level)
        {
            if (level < 0 || level >= QualitySettings.names.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Invalid quality level.");
            }

            QualitySettings.SetQualityLevel(level, true);
        }
    }
}