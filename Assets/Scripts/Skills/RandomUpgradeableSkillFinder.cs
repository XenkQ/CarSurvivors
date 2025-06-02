using System.Linq;

namespace Assets.Scripts.Skills
{
    public static class RandomUpgradeableSkillFinder
    {
        public static IUpgradeableSkill Find()
        {
            var skills = SkillsRegistry
                .Instance
                .Skills
                .Select(skill => skill as IUpgradeableSkill);

            if (skills.Count() == 0)
            {
                return null;
            }

            int randomSkillIndex = UnityEngine.Random.Range(0, skills.Count());
            int currentIndex = 0;
            foreach (var skill in skills)
            {
                if (currentIndex == randomSkillIndex)
                {
                    return skill;
                }
                currentIndex++;
            }

            return null;
        }
    }
}
