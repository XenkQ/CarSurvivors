using Assets.ScriptableObjects.Player.Skills;
using System.Linq;

namespace Assets.Scripts.Skills
{
    public class SkillUpgrader
    {
        public ISkillConfig GetRandomSkillConfigReadyForUpgrade()
        {
            var skills = SkillsManager.Instance.Skills;

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
                    return skill.CurrentConfig;
                }
                currentIndex++;
            }

            return null;
        }

        //public void UpgradeSkillConfig(ISkillConfig config, )
    }
}
