﻿using Assets.Scripts.Skills;
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
            List<NameUpgradableStatPair> upgradeableStats = new List<NameUpgradableStatPair>();
            PropertyInfo[] upgradeableStatsProperties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .Where(f => typeof(IUpgradeableStat).IsAssignableFrom(f.PropertyType))
                                          .ToArray();

            foreach (PropertyInfo property in upgradeableStatsProperties)
            {
                IUpgradeableStat upgradeableStat = (IUpgradeableStat)property.GetValue(this);
                if (upgradeableStat != null && upgradeableStat.CanBeUpgraded)
                {
                    upgradeableStats.Add(new NameUpgradableStatPair(property.Name, upgradeableStat));
                }
            }

            return upgradeableStats;
        }
    }
}
