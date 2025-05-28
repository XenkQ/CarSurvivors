using Assets.Scripts.Extensions;
using System;
using UnityEngine;
using Assets.ScriptableObjects.Player.Skills;
using Assets.ScriptableObjects;

namespace Assets.Scripts.Skills
{
    public interface ISkill : IInitializable
    {
        public StartEndScriptableConfig<SkillConfig> StartEndScriptableConfig { get; }
        public SkillConfig CurrentConfig { get; }

        public event EventHandler OnLevelUp;

        public void LevelUp();
    }

    public abstract class Skill<TConfig> : MonoBehaviour, ISkill
        where TConfig : SkillConfig
    {
        public event EventHandler OnLevelUp;

        public abstract StartEndScriptableConfig<SkillConfig> StartEndScriptableConfig { get; protected set; }
        public virtual SkillConfig CurrentConfig => _currentConfig;
        protected TConfig _currentConfig;

        public virtual void Initialize()
        {
            _currentConfig = (StartEndScriptableConfig.StartConfig as TConfig)?.Clone();
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
