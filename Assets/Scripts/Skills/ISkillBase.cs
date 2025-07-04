using Assets.ScriptableObjects.Skills;
using Assets.Scripts.Initializers;

namespace Assets.Scripts.Skills
{
    public interface ISkillBase : IInitializable
    {
        public SkillInfoSO SkillInfo { get; }
    }
}
