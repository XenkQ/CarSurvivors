using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public static class RandomDisabledSkillsActivator
    {
        public static void ActivateRandomDisabledSkill()
        {
            var inactiveSkills = SkillsRegistry.Instance.Skills
                .Select(skill => skill as IInitializable)
                .Where(skill => !skill.IsInitialized())
                .ToArray();

            if (inactiveSkills.Length > 0)
            {
                int index = Random.Range(0, inactiveSkills.Length);
                inactiveSkills[index].Initialize();
            }
        }
    }
}
