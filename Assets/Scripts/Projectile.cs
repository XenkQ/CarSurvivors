using Assets.ScriptableObjects;
using Assets.Scripts.Extensions;
using Assets.Scripts.Initializers;
using Assets.Scripts.LayerMasks;
using Assets.Scripts.StatusAffectables;
using UnityEngine;
using System;

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

        private float _distanceTraveled;
        private Vector3 _movementDir;

        private void FixedUpdate()
        {
            if (!_isAlive || !_isInitialized)
            {
                return;
            }

            HandleCollisions();
        }

        private void Update()
        {
            if (!_isAlive || !_isInitialized)
            {
                return;
            }

            MoveProjectileForward();
        }

        private void Start()
        {
            _movementDir = transform.forward;
        }

        public void Initialize(ProjectileConfigSO config)
        {
            _config = config;

            _piercedCounter = _config.MaxPiercing;

            transform.localScale = new Vector3(_config.Size, _config.Size, transform.localScale.y);

            _distanceTraveled = 0f;

            _isInitialized = true;
        }

        public bool IsInitialized() => _isInitialized;

        public bool SetMovementDirection(Vector3 direction)
        {
            if (direction.Equals(Vector3.zero))
            {
                return false;
            }

            _movementDir = direction.normalized;

            return true;
        }

        private void MoveProjectileForward()
        {
            float moveStep = _config.Speed * Time.deltaTime;
            transform.position += _movementDir * moveStep;
            _distanceTraveled += moveStep;

            if (_distanceTraveled >= _config.Range)
            {
                _isAlive = false;
                PlayEndLifeAnimation();
            }
        }

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
                    OnLifeEnd?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void PlayEndLifeAnimation()
        {
            transform.LifeEndingShrinkToZeroTween(
                _config.DisapearingDuration,
                () => OnLifeEnd?.Invoke(this, EventArgs.Empty)
            );
        }
    }
}
