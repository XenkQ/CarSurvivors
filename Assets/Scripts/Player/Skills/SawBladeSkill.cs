using System;
using Assets.Scripts.LayerMasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Player.Skills
{
    public class SawBladeSkill : Skill
    {
        [SerializeField] private float _knockbackPower = 2f;
        private float _knockbackMoveSpeed = 0.4f;
        [SerializeField] private float _damage;
        [SerializeField] private float _size;
        [SerializeField] private float _cooldown;
        private float _currentCooldown;
        private ushort _level;

        [SerializeField] private BoxCollider _boxCollider;

        public override event EventHandler OnLevelUp;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnEnable()
        {
            _currentCooldown = _cooldown;
        }

        public void Update()
        {
            if (_currentCooldown > 0)
            {
                _currentCooldown -= Time.deltaTime;
            }
            else
            {
                AtackAllEnemiesInsideCollider();
                _currentCooldown = _cooldown;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            AttackCollidingEntity(other);
        }

        public override void LevelUp()
        {
            _level++;
            OnLevelUp?.Invoke(this, EventArgs.Empty);
        }

        private void AtackAllEnemiesInsideCollider()
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + _boxCollider.center, _boxCollider.size, transform.rotation, EntityLayers.All);
            foreach (Collider collider in colliders)
            {
                AttackCollidingEntity(collider);
            }
        }

        private void AttackCollidingEntity(Collider other)
        {
            GameObject collisionObject = other.transform.gameObject;
            if (1 << collisionObject.layer == EntityLayers.Enemy)
            {
                if (collisionObject.TryGetComponent(out IDamageable damageable))
                {
                    collisionObject.transform.DOMove(other.transform.position + transform.forward * _knockbackPower, _knockbackMoveSpeed);
                    damageable.TakeDamage(_damage);
                }
            }
        }
    }
}
