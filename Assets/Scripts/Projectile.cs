using DG.Tweening;
using LayerMasks;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileStatsSO _stats;
    [SerializeField] private BoxCollider _boxCollider;
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
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + _boxCollider.center, _boxCollider.size,
                                          Vector3.down, transform.rotation, _boxCollider.size.y,
                                          EntityLayers.Enemy, QueryTriggerInteraction.Collide);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && hit.transform.gameObject.TryGetComponent(out IDamageable damagable))
            {
                damagable.TakeDamage(_stats.Damage);
                DecreasePiercing();
            }
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
