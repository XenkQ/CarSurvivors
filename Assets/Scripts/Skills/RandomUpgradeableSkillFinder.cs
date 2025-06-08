using System.Linq;

namespace Assets.Scripts.Skills
{
    public static class RandomUpgradeableSkillFinder
    {
        public static IUpgradeableSkill Find()
        {
            var upgradeableSkills = SkillsRegistry
                .Instance
                .Skills
                .Select(skill => skill as IUpgradeableSkill)
                .Where(skill => skill.CanBeUgraded());

            if (upgradeableSkills.Count() == 0)
            {
                return null;
            }

            int randomSkillIndex = UnityEngine.Random.Range(0, upgradeableSkills.Count());

            return upgradeableSkills.ElementAtOrDefault(randomSkillIndex);
        }
    }
}
