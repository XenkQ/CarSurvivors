using Assets.ScriptableObjects;
using Assets.Scripts.Extensions;
using Assets.Scripts.Initializers;
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
        private bool _isAlive = true;

        public event EventHandler OnLifeEnd;

        private void FixedUpdate()
        {
            if (!_isAlive || !_isInitialized)
            {
                return;
            }

            HandleCollisions();
        }

        public void Initialize(ProjectileConfigSO config)
        {
            _config = config;

            _piercedCounter = _config.MaxPiercing;

            StartMovingProjectileForward();

            transform.localScale = new Vector3(_config.Size, _config.Size, transform.localScale.y);

            _isInitialized = true;
        }

        public bool IsInitialized() => _isInitialized;

        private void HandleCollisions()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + _sphereCollider.center, _sphereCollider.radius,
                                         EntityLayers.Enemy | TerrainLayers.Impassable);
            foreach (Collider collider in colliders)
            {
                if (collider == null)
                {
                    continue;
                }

                EntityManipulationHelper.Damage(collider, _config.Damage);

                if (_piercedCounter > 0)
                {
                    _piercedCounter--;
                }
                else
                {
                    _isAlive = false;
                    DOTween.Kill(transform);
                    OnLifeEnd?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void StartMovingProjectileForward()
        {
            Vector3 targetPos = transform.position + transform.forward * _config.Range;
            targetPos.y = transform.position.y;
            transform
                .DOMove(targetPos, _config.TimeToArriveAtEndRangeMultiplier * _config.Range)
                .SetEase(Ease.Linear)
                .OnComplete(OnProjectileReachedDestination);
        }

        private void OnProjectileReachedDestination()
        {
            transform.LifeEndingShrinkToZeroTween(
                _config.DisapearingDuration,
                () => OnLifeEnd?.Invoke(this, EventArgs.Empty)
            );
        }
    }
}
