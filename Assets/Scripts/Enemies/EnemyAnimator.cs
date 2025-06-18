using Assets.Scripts.Animations;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator : MonoBehaviour, IAttackAnimationPlayer
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private float _animationResponseSpeed = 0.05f;
        public bool IsMovingByCrawling { get; set; }
        public bool IsPlayingAttackAnimation { get; private set; }
        private Animator _animator;

        public event EventHandler OnAttackAnimationStart;

        public event EventHandler OnAttackAnimationEnd;

        public event EventHandler OnAttackHitFrame;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _animator.SetBool("IsMovingByCrawling", IsMovingByCrawling);
            InvokeRepeating(nameof(HandleTransitionPropertiesChanges), 0, _animationResponseSpeed);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(HandleTransitionPropertiesChanges));
        }

        public void PlayAttackAnimation()
        {
            _animator.SetTrigger("Attack");
        }

        public void Call_OnAttackAnimationStart()
        {
            OnAttackAnimationStart?.Invoke(this, EventArgs.Empty);
            IsPlayingAttackAnimation = true;
        }

        public void Call_OnAttackAnimationEnd()
        {
            OnAttackAnimationEnd?.Invoke(this, EventArgs.Empty);
            IsPlayingAttackAnimation = false;
        }

        public void Call_OnAttackHitFrame()
        {
            OnAttackHitFrame?.Invoke(this, EventArgs.Empty);
        }

        private void HandleTransitionPropertiesChanges()
        {
            _animator.SetFloat("Speed", _enemy.MovementController.GetCurrentMovementSpeed());
        }
    }
}
