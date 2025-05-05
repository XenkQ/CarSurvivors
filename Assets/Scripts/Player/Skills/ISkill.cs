using System;

namespace Assets.Scripts.Player
{
    public interface ISkill
    {
        public event EventHandler OnLevelUp;

        public void LevelUp();

        public void Activate();
    }
}