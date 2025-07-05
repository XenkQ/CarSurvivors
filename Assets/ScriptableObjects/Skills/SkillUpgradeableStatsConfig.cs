using Assets.Scripts.Stats;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.ScriptableObjects.Player.Skills
{
    public readonly struct NameUpgradableStatPair
    {
        public string Name { get; }
        public IUpgradeableStat UpgradeableStat { get; }

        public NameUpgradableStatPair(string name, IUpgradeableStat upgradeableStat)
        {
            Name = name;
            UpgradeableStat = upgradeableStat;
        }
    }

    public interface ISkillUpgradeableStatsConfig
    {
        public IEnumerable<NameUpgradableStatPair> GetUpgradeableStatsThatCanBeUpgraded();
    }

    public abstract class SkillUpgradeableStatsConfig : ScriptableObject, ISkillUpgradeableStatsConfig
    {
        public IEnumerable<NameUpgradableStatPair> GetUpgradeableStatsThatCanBeUpgraded()
        {
            List<NameUpgradableStatPair> upgradeableStats = new();

            PropertyInfo[] upgradeableStatsPropertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .Where(f => typeof(IUpgradeableStat).IsAssignableFrom(f.PropertyType))
                                          .ToArray();

            foreach (PropertyInfo propertyInfo in upgradeableStatsPropertyInfos)
            {
                if (propertyInfo.GetValue(this) is IUpgradeableStat upgradeableStat
                    && upgradeableStat.CanBeUpgraded)
                {
                    upgradeableStats.Add(new NameUpgradableStatPair(propertyInfo.Name, upgradeableStat));
                }
            }

            return upgradeableStats;
        }
    }
}
