using Assets.Scripts.Settings;
using Assets.Scripts.Storage;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    public class GraphicSetting : ISetting<GraphicSetting, string>
    {
        public string DefaultValue => "High";

        public string GetKey()
        {
            return "GraphicsQuality";
        }

        public string GetValueOrStoredDefault()
        {
            if (AppStorage.TryGetValue(GetKey(), out string value))
            {
                return value;
            }

            return DefaultValue;
        }

        public void SaveValue(string value)
        {
            AppStorage.SetValue(GetKey(), value);
        }

        public void Load()
        {
            int savedLevel = Array.IndexOf(QualitySettings.names, GetValueOrStoredDefault());
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
