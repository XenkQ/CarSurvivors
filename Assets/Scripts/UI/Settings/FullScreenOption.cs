using Assets.Scripts.Settings;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI.Settings
{
    internal class FullScreenOption : MonoBehaviour, IOptionComponent<bool>
    {
        [Inject] private readonly FullScreenSetting _fullScreenSetting;

        [SerializeField] private Toggle _toggle;

        private void OnEnable()
        {
            LoadComponent();
            _toggle.RegisterValueChangedCallback(OnToogleValueChanged);
        }

        private void OnDisable()
        {
            _toggle.UnregisterValueChangedCallback(OnToogleValueChanged);
        }

        public void LoadComponent()
        {
            _fullScreenSetting.Load();
            _toggle.SetValueWithoutNotify(
                _fullScreenSetting.GetValueOrStoredDefault() == FullScreenMode.ExclusiveFullScreen);
        }

        public void PerformValueChange(bool value)
        {
            _fullScreenSetting.SaveValue(
                 value ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed
            );
            _fullScreenSetting.Load();
        }

        private void OnToogleValueChanged(ChangeEvent<bool> evt)
        {
            PerformValueChange(evt.newValue);
        }
    }
}