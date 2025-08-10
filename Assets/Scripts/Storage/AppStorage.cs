using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Storage
{
    public static class AppStorage
    {
        private static readonly string SettingsFilePath =
            Path.Combine(Application.persistentDataPath, "AppStorage.json");

        private static Dictionary<string, JToken> _settingsCache;

        static AppStorage()
        {
            Load();
        }

        public static bool TryGetValue<T>(string key, out T value)
        {
            if (_settingsCache.TryGetValue(key, out var result))
            {
                try
                {
                    value = result.ToObject<T>();
                    return true;
                }
                catch (Exception)
                {
                }
            }

            value = default;
            return false;
        }

        public static void SetValue<T>(string key, T value)
        {
            _settingsCache[key] = JToken.FromObject(value);
            var json = JsonConvert.SerializeObject(_settingsCache, Formatting.Indented);
            File.WriteAllText(SettingsFilePath, json);
        }

        private static void Load()
        {
            if (File.Exists(SettingsFilePath))
            {
                var json = File.ReadAllText(SettingsFilePath);
                var obj = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(json);
                _settingsCache = obj ?? new Dictionary<string, JToken>();
            }
            else
            {
                _settingsCache = new Dictionary<string, JToken>();
            }
        }
    }
}