using System;

namespace Assets.Scripts.Storage
{
    public interface IAppStorageValue<T>
    {
        public T DefaultValue { get; }

        public string GetKey();

        public T GetValueOrStoredDefault();

        public void SaveValue(T value);
    }
}