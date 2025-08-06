using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    public sealed class UserSettingsLoadManager : MonoBehaviour
    {
        private static UserSettingsLoadManager _instance;
        private static List<ISettingLoader> _settingLoaders;

        static UserSettingsLoadManager()
        {
            _settingLoaders = new()
            {
                new GraphicSetting(),
                new AudioVolumeSetting(),
            };
        }

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            foreach (var settingLoader in _settingLoaders)
            {
                settingLoader.Load();
            }
        }
    }
}