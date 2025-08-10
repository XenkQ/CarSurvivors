namespace Assets.Scripts.UI.Settings
{
    public interface IOptionComponent<T>
    {
        public void PerformValueChange(T value);

        public void LoadComponent();
    }
}