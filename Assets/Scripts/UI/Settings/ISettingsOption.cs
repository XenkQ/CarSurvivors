namespace Assets.Scripts.UI.Settings
{
    public interface ISettingsOption<T> : IPlayerPref
    {
        public void OnValueChanged(T value);

        public T GetValue();
    }
}
