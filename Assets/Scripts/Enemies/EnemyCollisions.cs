using Assets.Scripts.LayerMasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyCollisions : MonoBehaviour
    {
        [SerializeField] private float _collisionCheckDelay = 0.05f;
        [SerializeField] private float _collisionRadius = 1f;

        private List<Collider> _colliders;

        public event EventHandler<CollisionEventArgs> OnCollisionWithOtherEnemy;

        public event EventHandler<CollisionEventArgs> OnCollisionWithPlayer;

        private void Awake()
        {
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                if (collider.isTrigger)
                {
                    _colliders.Add(collider);
                }
            }
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(HandleCollisionsCheck), 0f, _collisionCheckDelay);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(HandleCollisionsCheck));
        }

        private void HandleCollisionsCheck()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _collisionRadius, Vector3.up, Mathf.Infinity, EntityLayers.All);
            foreach (var hit in hits)
            {
                Collider collider = hit.collider;
                if (hit.collider != null && !_colliders.Contains(hit.collider))
                {
                    if (1 << collider.gameObject.layer == EntityLayers.Enemy)
                    {
                        OnCollisionWithOtherEnemy?.Invoke(this, new CollisionEventArgs(collider));
                    }
                    else if (1 << collider.gameObject.layer == EntityLayers.Player)
                    {
                        OnCollisionWithPlayer?.Invoke(this, new CollisionEventArgs(collider));
                    }
                }
            }
        }
    }
}
