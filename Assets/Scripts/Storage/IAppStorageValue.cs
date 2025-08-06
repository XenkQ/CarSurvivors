namespace Assets.Scripts.Storage
{
    public interface IAppStorageValue<T>
    {
        public string GetKey();

        public T GetValue();

        public void SaveValue(T value);
    }
}