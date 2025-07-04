using UnityEngine;

namespace Assets.Scripts.Initializers
{
    public interface IInitializableWithScriptableConfig<TScriptableConfig>
        where TScriptableConfig : ScriptableObject
    {
        public void Initialize(TScriptableConfig config);

        public bool IsInitialized();
    }
}
