using System;
using UnityEngine;
using Assets.ScriptableObjects.Player.Skills;

namespace Assets.Scripts.Skills
{
    public interface IConfigurableSkill<TConfig> : IConfigurableByScriptableObject<TConfig>, ISkillBase
        where TConfig : SkillUpgradableConfig
    {
    }

    public abstract class ConfigurableSkill<TConfig> : MonoBehaviour, IConfigurableSkill<TConfig>
        where TConfig : SkillUpgradableConfig
    {
        public abstract TConfig Config { get; protected set; }

        public event EventHandler OnLevelUp;

        public virtual void Initialize()
        {
            gameObject.SetActive(true);
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
