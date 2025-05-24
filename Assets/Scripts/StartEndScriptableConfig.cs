using System;
using UnityEngine;

namespace Assets.Scripts.Player.Skills
{
    public interface IStartEndScriptableConfig<T> where T : ScriptableObject
    {
        public T StartConfig { get; }
        public T EndConfig { get; }
    }

    [Serializable]
    public class StartEndScriptableConfig<T> : IStartEndScriptableConfig<T> where T : ScriptableObject
    {
        [field: SerializeField] public T StartConfig { get; set; }

        [field: SerializeField] public T EndConfig { get; set; }
    }
}
