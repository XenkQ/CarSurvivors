using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public static class RandomUninitializedSkillsInitializator
    {
        public static ISkillBase Initialize()
        {
            if (SkillsRegistry.Instance.UninitializedSkillsCount > 0)
            {
                var inactiveSkills = SkillsRegistry.Instance.GetUninitializedSkills().ToArray();
                int index = Random.Range(0, inactiveSkills.Length);
                ISkillBase inactiveSkill = inactiveSkills[index];

                if (inactiveSkill != null)
                {
                    return SkillsRegistry.Instance.InitializeSkill(inactiveSkill);
                }
            }

            return null;
        }
    }
}
