using System;

namespace Assets.Scripts.Animations
{
    public interface IAttackAnimationPlayer
    {
        public bool IsPlayingAttackAnimation { get; }

        public event EventHandler OnAttackAnimationStart;

        public event EventHandler OnAttackAnimationEnd;

        public event EventHandler OnAttackHitFrame;

        public void PlayAttackAnimation();
    }
}
