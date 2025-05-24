using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public class SkillsManager : MonoBehaviour
    {
        public IEnumerable<ISkill> Skills { get; private set; }

        private void Awake()
        {
            SetAllSkills();
        }

        private void Start()
        {
#if DEBUG
            InitializeAllSkills();
#endif
        }

        private void SetAllSkills()
        {
            GameObject[] skillGameObjects = GameObject.FindGameObjectsWithTag("Skill");
            List<ISkill> skills = new List<ISkill>();

            foreach (GameObject gameObject in skillGameObjects)
            {
                if (gameObject.TryGetComponent(out ISkill skill))
                {
                    skills.Add(skill);
                }
            }

            Skills = skills;
        }

        private void InitializeAllSkills()
        {
            foreach (var skill in Skills)
            {
                skill.Initialize();
            }
        }

        private void ActivateRandomDisabledSkill()
        {
            ISkill[] inactiveSkills = Skills.Where(skill => !skill.IsInitialized()).ToArray();
            if (inactiveSkills.Length > 0)
            {
                int index = Random.Range(0, inactiveSkills.Length);
                inactiveSkills[index].Initialize();
            }
        }
    }
}
