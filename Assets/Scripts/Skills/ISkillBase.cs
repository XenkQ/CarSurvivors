using System;

namespace Assets.Scripts.Skills
{
    public interface ISkillBase : IInitializable
    {
        public event EventHandler OnLevelUp;

        public void LevelUp();
    }
}
