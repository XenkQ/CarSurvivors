using System;

namespace Assets.Scripts.Skills
{
    public interface ISkill : IInitializable
    {
        public event EventHandler OnLevelUp;

        public void LevelUp();
    }
}
