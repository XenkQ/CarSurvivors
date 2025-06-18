using Assets.Scripts.Animations;
using Assets.Scripts.Collisions;
using Assets.Scripts.Extensions;
using Assets.Scripts.StatusAffectables;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private float _attackRange = 1f;
        private Enemy _enemy;
        private IAttackAnimationPlayer _attackAnimationPlayer;
        private Collider _currentAttackedTarget;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _attackAnimationPlayer = _enemy.EnemyAnimator as IAttackAnimationPlayer;
        }

        private void OnEnable()
        {
            _enemy.CollisionsController.OnCollisionWithPlayer += EnemyCollisions_OnCollisionWithPlayer;
            _attackAnimationPlayer.OnAttackHitFrame += (s, e) => DamageCurrentlyAttackedTarget();
        }

        private void OnDisable()
        {
            _enemy.CollisionsController.OnCollisionWithPlayer -= EnemyCollisions_OnCollisionWithPlayer;
        }

        private void EnemyCollisions_OnCollisionWithPlayer(object sender, CollisionEventArgs e)
        {
            _enemy.EnemyAnimator.PlayAttackAnimation();
            _currentAttackedTarget = e.Collider;
        }

        private void DamageCurrentlyAttackedTarget()
        {
            if (CanAttackCurrentAttackTarget()
                && _currentAttackedTarget.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_enemy.Config.Damage);
                _currentAttackedTarget = null;
            }
        }

        private bool CanAttackCurrentAttackTarget()
        {
            if (_currentAttackedTarget == null)
            {
                return false;
            }

            RaycastHit[] hits = new RaycastHit[10];

            Physics.SphereCastNonAlloc(
                transform.position,
                _attackRange,
                Vector3.up,
                hits,
                _attackRange,
                new LayerMask().LayerToMask(_currentAttackedTarget.gameObject.layer));

            return hits.Any(hit => hit.collider != null && hit.collider.gameObject == _currentAttackedTarget.gameObject);
        }

        private void OnDrawGizmos()
        {
            const int numberOfSegments = 8;
            new Debug().DrawCircle(transform.position, _attackRange, numberOfSegments, Color.red);
        }
    }
}
