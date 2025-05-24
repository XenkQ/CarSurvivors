using UnityEngine;

namespace Assets.Scripts
{
    public interface IInitializable
    {
        public void Initialize();

        public bool IsInitialized();
    }

    public interface IInitializable<TScriptableConfig>
        where TScriptableConfig : ScriptableObject
    {
        public void Initialize(TScriptableConfig config);

        public bool IsInitialized();
    }
}
