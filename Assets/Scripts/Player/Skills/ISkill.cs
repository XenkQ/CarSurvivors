using System;

namespace Player.Skills
{
    public interface ISkill
    {
        public event EventHandler OnLevelUp;

        public void LevelUp();

        public void Activate();
    }
}
