using UnityEngine;

namespace Assets.ScriptableObjects
{
    public interface IConfigurableByScriptableObject<TConfig>
        where TConfig : ScriptableObject
    {
        public TConfig Config { get; }
    }
}
