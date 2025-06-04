using System;
using UnityEngine;
using Assets.ScriptableObjects.Player.Skills;
using Assets.ScriptableObjects.Skills;

namespace Assets.Scripts.Skills
{
    public interface IUpgradeableSkill : ISkillBase
    {
        public ISkillUpgradeableStatsConfig Config { get; }

        public event EventHandler OnUpgrade;
    }

    public abstract class UpgradeableSkill<TUpgradeableConfig> : MonoBehaviour, IUpgradeableSkill
        where TUpgradeableConfig : ISkillUpgradeableStatsConfig
    {
        public abstract SkillInfoSO SkillInfo { get; protected set; }
        protected abstract TUpgradeableConfig _config { get; set; }
        public ISkillUpgradeableStatsConfig Config => _config;

        public event EventHandler OnUpgrade;

        public virtual void Initialize()
        {
            gameObject.SetActive(true);
        }

        public virtual bool IsInitialized()
        {
            return gameObject.activeSelf;
        }
    }
}
