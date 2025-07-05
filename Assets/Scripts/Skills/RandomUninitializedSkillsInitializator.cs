using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public static class RandomUninitializedSkillsInitializator
    {
        public static ISkillBase Initialize(ISkillsRegistry skillsRegistry)
        {
            if (skillsRegistry.UninitializedSkillsCount > 0)
            {
                var inactiveSkills = skillsRegistry.GetUninitializedSkills().ToArray();
                int index = Random.Range(0, inactiveSkills.Length);
                ISkillBase inactiveSkill = inactiveSkills[index];

                if (inactiveSkill != null)
                {
                    return skillsRegistry.InitializeSkill(inactiveSkill);
                }
            }

            return null;
        }
    }
}
