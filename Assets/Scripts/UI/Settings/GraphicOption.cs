using Assets.Scripts.Storage;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Settings
{
    public class GraphicOption : MonoBehaviour, ISettingsOption<string>
    {
        private Dictionary<string, int> _qualityLevels = new Dictionary<string, int>
        {
            { "Low", 0 },
            { "Medium", 1 },
            { "High", 2 },
            { "Ultra", 3 }
        };

        [SerializeField] private TMP_Dropdown _dropDown;

        private string _qualityLevel = "Low";

        public string GetValue()
        {
            return _qualityLevel;
        }

        private void OnEnable()
        {
            LoadValue();
            _dropDown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            SaveValue();
            _dropDown.onValueChanged.RemoveListener(OnValueChanged);
        }

        public void LoadValue()
        {
            _qualityLevel = AppStorage.Get<string>(GraphicQualityManager.GRAPHICS_QUALITY_KEY);
            _dropDown.SetValueWithoutNotify(_qualityLevels[_qualityLevel]);
        }

        public void SaveValue()
        {
            AppStorage.Set(GraphicQualityManager.GRAPHICS_QUALITY_KEY, _qualityLevel);
        }

        public void OnValueChanged(int value)
        {
            var pair = _qualityLevels.FirstOrDefault(x => x.Value == value);

            if (pair.Equals(default(KeyValuePair<string, int>)))
            {
                return;
            }

            OnValueChanged(pair.Key);
        }

        public void OnValueChanged(string value)
        {
            GraphicQualityManager.Instance.SetQualityLevel(_qualityLevels[value]);
            _qualityLevel = value;
        }
    }
}
