using UnityEngine;

namespace Assets.Scripts.Skills
{
    public interface IConfigurableByScriptableObject<TConfig>
        where TConfig : ScriptableObject
    {
        public TConfig Config { get; }
    }
}
