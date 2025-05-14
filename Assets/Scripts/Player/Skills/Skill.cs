using System;
using UnityEngine;

namespace Assets.Scripts.Player.Skills
{
    public abstract class Skill : MonoBehaviour, ISkill
    {
        public abstract event EventHandler OnLevelUp;

        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }

        public abstract void LevelUp();
    }
}
