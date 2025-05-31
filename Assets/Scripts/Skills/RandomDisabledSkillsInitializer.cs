using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public static class RandomDisabledSkillsInitializer
    {
        public static ISkillBase InitializeRandomUninitializedSkill()
        {
            var inactiveSkills = SkillsRegistry.Instance.GetUninitializedSkills()
                .Where(skill => skill is IInitializable)
                .ToArray();

            if (inactiveSkills.Length > 0)
            {
                int index = Random.Range(0, inactiveSkills.Length);
                inactiveSkills[index].Initialize();
                return inactiveSkills[index] as ISkillBase;
            }

            return null;
        }
    }
}
