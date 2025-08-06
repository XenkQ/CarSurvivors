using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    public sealed class UserSettingsLoader : MonoBehaviour
    {
        [Inject] private readonly IEnumerable<ISettingLoader> _settingLoaders;
        private static UserSettingsLoader _instance;

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