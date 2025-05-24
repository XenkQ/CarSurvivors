using Assets.Scripts.Player.Skills;
using Assets.Scripts.Extensions;
using System;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public abstract class Skill<TScriptableConfig> : MonoBehaviour, ISkill
        where TScriptableConfig : ScriptableObject
    {
        public abstract StartEndScriptableConfig<TScriptableConfig> StartEndScriptableConfig { get; protected set; }
        public abstract TScriptableConfig CurrentConfig { get; set; }

        public event EventHandler OnLevelUp;

        public virtual void Initialize()
        {
            CurrentConfig = StartEndScriptableConfig.StartConfig.Clone();
        }

        public virtual bool IsInitialized()
        {
            return gameObject.activeSelf;
        }

        public virtual void LevelUp()
        {
            OnLevelUp?.Invoke(this, EventArgs.Empty);
        }
    }
}
