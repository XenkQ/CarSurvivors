using System;
using LayerMasks;
using UnityEngine;

public class SawBladeSkill : Skill
{
    [SerializeField] private float _damage;
    [SerializeField] private float _size;
    [SerializeField] private float _cooldown;
    private float _currentCooldown;
    private int _level;

    [SerializeField] private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _currentCooldown = _cooldown;
    }

    public override event EventHandler OnLevelUp;

    public override void LevelUp()
    {
        _level++;
    }

    public void Update()
    {
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
        }
        else
        {
            RaycastHit[] colliders = Physics.BoxCastAll(transform.position + _boxCollider.center, _boxCollider.size, Vector3.down, transform.rotation);
            foreach (RaycastHit collider in colliders)
            {
                GameObject collisionObject = collider.transform.gameObject;
                Debug.Log(collisionObject.name);
                if ((1 << collisionObject.layer) == EntityLayers.Enemy)
                {
                    if (collisionObject.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(_damage);
                    }
                }
            }
            _currentCooldown = _cooldown;
        }
    }
}