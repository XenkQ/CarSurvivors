using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private float _animationResponseSpeed = 0.05f;
        public bool IsMovingByCrawling { get; set; }
        public bool IsPlayingAttackAnimation { get; private set; }
        private Animator _animator;

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

        public void OnAttackAnimationStart()
        {
            IsPlayingAttackAnimation = true;
        }

        public void OnAttackAnimationEnd()
        {
            IsPlayingAttackAnimation = false;
        }

        private void HandleTransitionPropertiesChanges()
        {
            _animator.SetFloat("Speed", _enemy.MovementController.GetCurrentMovementSpeed());
        }
    }
}
