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

        public static T Get<T>(string key, Action<Exception> onError = null)
        {
            if (_settingsCache.TryGetValue(key, out var value))
            {
                try
                {
                    return value.ToObject<T>();
                }
                catch (Exception ex)
                {
                    onError?.Invoke(new InvalidOperationException($"Failed to convert setting '{key}' to type {typeof(T)}", ex));
                }
            }
            else
            {
                onError?.Invoke(new KeyNotFoundException($"Setting '{key}' not found in settings file."));
            }

            return default;
        }

        public static void Set<T>(string key, T value)
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