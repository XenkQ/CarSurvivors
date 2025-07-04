using Assets.Scripts.Initializers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public sealed class SkillsRegistry : MonoBehaviour
    {
        public IReadOnlyList<ISkillBase> Skills { get; private set; }
        public static SkillsRegistry Instance { get; private set; }
        public byte UninitializedSkillsCount { get; private set; }

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

            RegisterAllSkills();
        }

        public void Start()
        {
            UninitializedSkillsCount = (byte)GetUninitializedSkills().Count();

            InitializeSkill(Skills[0]);
        }

        private void RegisterAllSkills()
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

        public IEnumerable<ISkillBase> GetUninitializedSkills()
        {
            return Skills
                    .Select(skill => skill as IInitializable)
                    .Where(skill => !skill.IsInitialized())
                    .Select(skill => skill as ISkillBase)
                    .ToArray();
        }

        public ISkillBase InitializeSkill(ISkillBase skill)
        {
            if (skill is IInitializable initializableSkill && !initializableSkill.IsInitialized())
            {
                initializableSkill.Initialize();

                if (UninitializedSkillsCount - 1 >= 0)
                {
                    UninitializedSkillsCount--;
                }

                return skill;
            }

            return null;
        }
    }
}
