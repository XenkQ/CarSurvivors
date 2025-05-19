using System;

namespace Assets.Scripts.Player.Skills
{
    public interface ISkill
    {
        public event EventHandler OnLevelUp;

        public void LevelUp();

        public void Activate();

        public bool IsActive();
    }
}
