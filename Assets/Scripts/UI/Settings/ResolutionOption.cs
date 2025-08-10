using System.Linq;
using Assets.Scripts.Settings;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Settings
{
    public class ResolutionOption : MonoBehaviour, IOptionComponent<int>
    {
        [Inject] private readonly ResolutionSetting _resolutionSetting;

        [SerializeField] private TMP_Dropdown _resolutionDropdown;

        private void Awake()
        {
            SetDropdownOptions();
        }

        private void OnEnable()
        {
            LoadComponent();
            _resolutionDropdown.onValueChanged.AddListener(PerformValueChange);
        }

        private void OnDisable()
        {
            _resolutionDropdown.onValueChanged.RemoveListener(PerformValueChange);
        }

        public void LoadComponent()
        {
            _resolutionDropdown.SetValueWithoutNotify(
                Screen.resolutions.ToList().IndexOf(_resolutionSetting.GetValueOrStoredDefault())
            );
        }

        public void PerformValueChange(int value)
        {
            _resolutionSetting.SaveValue(Screen.resolutions[value]);
            _resolutionSetting.Load();
        }

        private void SetDropdownOptions()
        {
            _resolutionDropdown.ClearOptions();
            var options = Screen.resolutions.Select(r => $"{r.width} x {r.height}").ToList();
            _resolutionDropdown.AddOptions(options);
        }
    }
}