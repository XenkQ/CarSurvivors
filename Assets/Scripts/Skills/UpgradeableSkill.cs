using UnityEngine;
using Assets.ScriptableObjects.Player.Skills;
using Assets.ScriptableObjects.Skills;
using System.Linq;

namespace Assets.Scripts.Skills
{
    public interface IUpgradeableSkill : ISkillBase
    {
        public bool CanBeUgraded();

        public ISkillUpgradeableStatsConfig Config { get; }
    }

    public abstract class UpgradeableSkill<TUpgradeableConfig> : MonoBehaviour, IUpgradeableSkill
        where TUpgradeableConfig : ISkillUpgradeableStatsConfig
    {
        public abstract SkillInfoSO SkillInfo { get; protected set; }
        protected abstract TUpgradeableConfig _config { get; set; }
        public ISkillUpgradeableStatsConfig Config => _config;

        protected bool _isInitialized;

        protected virtual void OnEnable()
        {
            if (gameObject.activeSelf)
            {
                _isInitialized = true;
            }
        }

        public virtual bool CanBeUgraded()
        {
            return _config != null && _config.GetUpgradeableStatsThatCanBeUpgraded().Count() > 0;
        }

        public virtual void Initialize()
        {
            gameObject.SetActive(true);
            _isInitialized = true;
        }

        public virtual bool IsInitialized()
            => _isInitialized;
    }
}
