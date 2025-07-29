using Assets.Scripts.Storage;

namespace Assets.Scripts.UI.Settings
{
    public interface ISettingsOption<T> : IStoredValue
    {
        public void OnValueChanged(T value);

        public T GetValue();
    }
}
