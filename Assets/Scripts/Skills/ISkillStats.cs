using System.Collections.Generic;

namespace Assets.Scripts.Skills
{
    public interface ISkillStats : IUpgradable
    {
        public IEnumerable<string> GetStatsFieldNames();
    }
}
