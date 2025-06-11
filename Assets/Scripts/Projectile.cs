using Assets.ScriptableObjects;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.StatusAffectables;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Projectile : MonoBehaviour, IInitializableWithScriptableConfig<ProjectileConfigSO>
    {
        [SerializeField] private ProjectileConfigSO _config;
        [SerializeField] private SphereCollider _sphereCollider;
        private byte _piercedCounter;
        private bool _isInitialized;

        public event EventHandler OnLifeEnd;

        private void FixedUpdate()
        {
            if (_isInitialized)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position + _sphereCollider.center, _sphereCollider.radius,
                                             EntityLayers.Enemy | TerrainLayers.Impassable);
                foreach (Collider collider in colliders)
                {
                    if (collider == null)
                    {
                        return;
                    }

                    if (collider.transform.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(_config.Damage);
                    }

                    DecreasePiercing();
                }
            }
        }

        public void Initialize(ProjectileConfigSO config)
        {
            _config = config;

            _piercedCounter = _config.MaxPiercing;

            StartMovingBulletForward();

            _isInitialized = true;
        }

        public bool IsInitialized() => _isInitialized;

        private void StartMovingBulletForward()
        {
            Vector3 targetPos = transform.position + transform.forward * _config.Range;
            targetPos.y = transform.position.y;
            transform
                .DOMove(targetPos, _config.TimeToArriveAtEndRangeMultiplier * _config.Range)
                .SetEase(Ease.Linear)
                .OnComplete(EndLife);
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
