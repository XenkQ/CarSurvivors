using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(EnemyMovementController))]
    public class EnemyAnimationsController : MonoBehaviour
    {
        [SerializeField] private bool _isMovingByCrawling;
        [SerializeField] private float _animationResponseSpeed = 0.05f;
        [SerializeField] private Animator animator;
        private IMovementController _movementController;

        private void Awake()
        {
            _movementController = GetComponent<IMovementController>();
        }

        private void OnEnable()
        {
            animator.SetBool("IsMovingByCrawling", _isMovingByCrawling);
            InvokeRepeating(nameof(HandleAnimations), 0, _animationResponseSpeed);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(HandleAnimations));
        }

        private void HandleAnimations()
        {
            animator.SetFloat("Speed", _movementController.GetCurrentMovementSpeed());
        }

        public void PlayAttackAnimation()
        {
            animator.SetTrigger("Attack");
        }
    }
}
