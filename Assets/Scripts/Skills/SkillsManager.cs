using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public sealed class SkillsManager : MonoBehaviour
    {
        public IEnumerable<ISkill> Skills { get; private set; }
        public static SkillsManager Instance { get; private set; }

        private SkillsManager()
        { }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            SetAllSkills();
        }

        private void Start()
        {
#if DEBUG
            InitializeAllSkills();
            Debug.Log("Skills count: " + Skills.Count());
#endif
        }

        private void SetAllSkills()
        {
            var skills = new List<ISkill>();

            foreach (Transform skillChild in transform)
            {
                if (skillChild.gameObject.TryGetComponent(out ISkill skill))
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
            var inactiveSkills = Skills.Where(skill => !skill.IsInitialized()).ToArray();
            if (inactiveSkills.Length > 0)
            {
                int index = Random.Range(0, inactiveSkills.Length);
                inactiveSkills[index].Initialize();
            }
        }
    }
}
