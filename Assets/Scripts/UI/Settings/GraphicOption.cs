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
    public class GraphicOption : MonoBehaviour, IOptionComponent<int>
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

        private void OnEnable()
        {
            LoadComponent();

            _dropDown.onValueChanged.AddListener(PerformValueChange);
        }

        private void OnDisable()
        {
            _dropDown.onValueChanged.RemoveListener(PerformValueChange);
        }

        public void PerformValueChange(int value)
        {
            var pair = _qualityLevels.FirstOrDefault(x => x.Value == value);

            if (pair.Equals(default(KeyValuePair<string, int>)))
            {
                return;
            }

            _graphicSetting.SaveValue(pair.Key);
            _graphicSetting.Load();
        }

        public void LoadComponent()
        {
            _graphicSetting.Load();

            string qualityLevel = _graphicSetting.GetValueOrStoredDefault();

            _dropDown.SetValueWithoutNotify(_qualityLevels[qualityLevel]);
        }
    }
}