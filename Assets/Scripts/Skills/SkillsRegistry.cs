using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public sealed class SkillsRegistry : MonoBehaviour
    {
        public IEnumerable<ISkillBase> Skills { get; private set; }
        public static SkillsRegistry Instance { get; private set; }

        private SkillsRegistry()
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
            InitializeAllSkills();
        }

        private void SetAllSkills()
        {
            var skills = new List<ISkillBase>();

            foreach (Transform skillChild in transform)
            {
                if (skillChild.gameObject.TryGetComponent(out ISkillBase skill))
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
                if (skill is IInitializable initializable)
                {
                    initializable.Initialize();
                }
            }
        }
    }
}
