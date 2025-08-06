using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Assets.Scripts.CustomEventArgs;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Settings
{
    public class GraphicOption : MonoBehaviour
    {
        public event EventHandler<ValueEventArgs<string>> OnValueChanged;

        [Inject] private readonly GraphicSetting _graphicSetting;

        [SerializeField] private TMP_Dropdown _dropDown;

        private IReadOnlyDictionary<string, int> _qualityLevels = new ReadOnlyDictionary<string, int>(
            new Dictionary<string, int>()
            {
                { "Low", 0 },
                { "Medium", 1 },
                { "High", 2 },
                { "Ultra", 3 }
            }
        );

        private string _qualityLevel = "High";

        private void OnEnable()
        {
            LoadComponent();

            _dropDown.onValueChanged.AddListener(PerformValueChange);
        }

        private void OnDisable()
        {
            _graphicSetting.SaveValue(_qualityLevel);
            _dropDown.onValueChanged.RemoveListener(PerformValueChange);
        }

        public void PerformValueChange(int value)
        {
            var pair = _qualityLevels.FirstOrDefault(x => x.Value == value);

            if (pair.Equals(default(KeyValuePair<string, int>)))
            {
                return;
            }

            _qualityLevel = pair.Key;
            _graphicSetting.SaveValue(_qualityLevel);
            _graphicSetting.Load();
        }

        private void LoadComponent()
        {
            _graphicSetting.Load();
            _qualityLevel = _graphicSetting.GetValue();
            _dropDown.SetValueWithoutNotify(_qualityLevels[_qualityLevel]);
        }
    }
}