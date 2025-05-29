using System;
using UnityEngine;
using Assets.ScriptableObjects.Player.Skills;

namespace Assets.Scripts.Skills
{
    public interface IUpgradeableSkill<TUpgradeableConfig> : IConfigurableByScriptableObject<TUpgradeableConfig>, ISkillBase
        where TUpgradeableConfig : SkillUpgradeableConfig
    {
        public event EventHandler OnUpgrade;
    }

    public abstract class UpgradeableSkill<TUpgradeableConfig> : MonoBehaviour, IUpgradeableSkill<TUpgradeableConfig>
        where TUpgradeableConfig : SkillUpgradeableConfig
    {
        public abstract TUpgradeableConfig Config { get; protected set; }

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
