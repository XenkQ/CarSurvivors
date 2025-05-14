using Assets.Scripts.LayerMasks;
using DG.Tweening;
using Scripts;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileStatsSO _stats;
        [SerializeField] private SphereCollider _sphereCollider;
        private ushort _piercedCounter;

        public event EventHandler OnLifeEnd;

        private void OnEnable()
        {
            _piercedCounter = _stats.MaxPiercing;
            Vector3 targetPos = transform.position + transform.forward * _stats.Range;
            targetPos.y = transform.position.y;
            transform.DOMove(targetPos, _stats.TimeToArriveAtRangeEnd).SetEase(Ease.Linear).OnComplete(EndLife);
        }

        private void FixedUpdate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + _sphereCollider.center, _sphereCollider.radius,
                                                         EntityLayers.Enemy | TerrainLayers.Impassable);
            foreach (Collider collider in colliders)
            {
                if (collider == null)
                {
                    return;
                }

                if (collider.transform.gameObject.TryGetComponent(out IDamageable damagable))
                {
                    damagable.TakeDamage(_stats.Damage);
                }

                DecreasePiercing();
            }
        }

        private void EndLife()
        {
            OnLifeEnd?.Invoke(this, EventArgs.Empty);
        }

        private void DecreasePiercing()
        {
            if (_piercedCounter > 0)
            {
                _piercedCounter--;
            }
            else
            {
                EndLife();
            }
        }
    }
}
