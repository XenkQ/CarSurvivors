using System;
using UnityEngine;

namespace Player.Skills
{
    public class MinigunSkill : Skill
    {
        //I think miniguns will be rotating from min angle to max angle and shooting (not by targetting specific enemy) shoots will pierce them
        [SerializeField] private float _knockbackPower = 2f;
        private float _knockbackMoveSpeed = 0.4f;
        [SerializeField] private float _damage;
        [SerializeField] private float _size;
        [SerializeField] private float _cooldown;

        public override event EventHandler OnLevelUp;

        public override void LevelUp()
        {
            throw new NotImplementedException();
        }
    }
}
