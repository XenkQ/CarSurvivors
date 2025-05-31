using Assets.Scripts.CustomTypes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.ScriptableObjects.Player.Skills
{
    public interface ISkillUpgradeableStatsConfig
    {
        public IEnumerable<IUpgradeableStat> GetUpgradeableStats();
    }

    public class SkillUpgradeableStatsConfig : ScriptableObject, ISkillUpgradeableStatsConfig
    {
        public IEnumerable<IUpgradeableStat> GetUpgradeableStats()
        {
            List<IUpgradeableStat> upgradeableStats = new List<IUpgradeableStat>();
            FieldInfo[] upgradeableStatsFields = GetType().GetFields(BindingFlags.Public)
                                          .Where(f => typeof(IUpgradeableStat).IsAssignableFrom(f.FieldType))
                                          .ToArray();
            return upgradeableStatsFields
                .Select(f => f.GetValue(this))
                .OfType<IUpgradeableStat>()
                .ToList();
        }
    }
}
