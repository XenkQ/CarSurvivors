namespace Assets.Scripts
{
    public interface IInitializable
    {
        public void Initialize();

        public bool IsInitialized();
    }

    public interface IInitializable<T>
    {
        public void Initialize(T input);

        public bool IsInitialized();
    }
}
