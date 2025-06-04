using Assets.ScriptableObjects.Skills;

namespace Assets.Scripts.Skills
{
    public interface ISkillBase : IInitializable
    {
        public SkillInfoSO SkillInfo { get; }
    }
}
