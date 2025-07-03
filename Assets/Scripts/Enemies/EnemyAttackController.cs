﻿using Assets.Scripts.Animations;
using Assets.Scripts.Collisions;
using Assets.Scripts.Extensions;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.StatusAffectables;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField, Range(0, 360)] private float _attackArcAngle = 60f;
        [SerializeField] private float _attackRange = 1f;
        private Enemy _enemy;
        private IAttackAnimationPlayer _attackAnimationPlayer;
        private Collider _currentAttackedTarget;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _attackAnimationPlayer = _enemy.EnemyAnimator;
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
            if (_attackAnimationPlayer.IsPlayingAttackAnimation)
            {
                return;
            }

            _currentAttackedTarget = e.Collider;

            if (CanAttackCurrentAttackTarget())
            {
                _enemy.EnemyAnimator.PlayAttackAnimation();
            }
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

            Collider attackedTarget = GetAttackedColliderIfInRange();

            if (attackedTarget == _currentAttackedTarget)
            {
                Vector3 toTarget = GetComponent<Collider>().ClosestPoint(transform.position) - transform.position;
                toTarget.y = 0f;

                if (toTarget.sqrMagnitude < 0.001f)
                {
                    return true;
                }

                float angleToTarget = Vector3.Angle(transform.forward, toTarget);
                if (angleToTarget <= _attackArcAngle * 0.5f)
                {
                    float distanceToTarget = toTarget.magnitude;

                    Ray ray = new Ray(transform.position, toTarget.normalized);
                    if (Physics.Raycast(ray, out RaycastHit rayHit, distanceToTarget, TerrainLayers.All)
                        && rayHit.collider != _currentAttackedTarget)
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        private Collider GetAttackedColliderIfInRange()
        {
            return Physics.OverlapSphere(
                transform.position,
                _attackRange,
                1 << _currentAttackedTarget.gameObject.layer
            ).FirstOrDefault();
        }

        private void OnDrawGizmos()
        {
            const int SEGMENTS = 16;

            new Debug().DrawArc(
                transform.position,
                transform.forward,
                _attackArcAngle,
                _attackRange,
                SEGMENTS,
                Color.red);
        }
    }
}
