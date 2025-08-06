using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CustomEventArgs;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Settings
{
    public class GraphicOption : MonoBehaviour, ISettingsOption<string>
    {
        public event EventHandler<ValueEventArgs<string>> OnValueChanged;

        [SerializeField] private TMP_Dropdown _dropDown;

        private Dictionary<string, int> _qualityLevels = new Dictionary<string, int>
        {
            { "Low", 0 },
            { "Medium", 1 },
            { "High", 2 },
            { "Ultra", 3 }
        };

        private string _qualityLevel = "High";

        private void OnEnable()
        {
            _dropDown.onValueChanged.AddListener(PerformValueChange);
        }

        private void OnDisable()
        {
            _dropDown.onValueChanged.RemoveListener(PerformValueChange);
        }

        public string GetValue()
        {
            return _qualityLevel;
        }

        public void PerformValueChange(int value)
        {
            var pair = _qualityLevels.FirstOrDefault(x => x.Value == value);

            if (pair.Equals(default(KeyValuePair<string, int>)))
            {
                return;
            }

            OnValueChanged?.Invoke(this, new(pair.Key));
        }
    }
}