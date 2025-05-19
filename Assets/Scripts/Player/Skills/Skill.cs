using System;
using UnityEngine;

namespace Assets.Scripts.Player.Skills
{
    public abstract class Skill : MonoBehaviour, ISkill
    {
        public event EventHandler OnLevelUp;

        protected ushort _level;

        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }

        public virtual void LevelUp()
        {
            _level++;
            OnLevelUp?.Invoke(this, EventArgs.Empty);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
    }
}
