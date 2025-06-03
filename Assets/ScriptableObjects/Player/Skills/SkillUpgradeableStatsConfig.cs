using Assets.Scripts.CustomTypes;
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
        public IEnumerable<NameUpgradableStatPair> GetUpgradeableStats();
    }

    public abstract class SkillUpgradeableStatsConfig : ScriptableObject, ISkillUpgradeableStatsConfig
    {
        public IEnumerable<NameUpgradableStatPair> GetUpgradeableStats()
        {
            List<NameUpgradableStatPair> upgradeableStats = new List<NameUpgradableStatPair>();
            PropertyInfo[] upgradeableStatsProperties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .Where(f => typeof(IUpgradeableStat).IsAssignableFrom(f.PropertyType))
                                          .ToArray();

            foreach (PropertyInfo property in upgradeableStatsProperties)
            {
                IUpgradeableStat upgradeableStat = (IUpgradeableStat)property.GetValue(this);
                if (upgradeableStat != null)
                {
                    upgradeableStats.Add(new NameUpgradableStatPair(property.Name, upgradeableStat));
                }
            }

            return upgradeableStats;
        }
    }
}
