using Assets.Scripts.LayerMasks;
using UnityEngine;

namespace Assets.Scripts.Player.Skills.LandmineTrap
{
    public class Landmine : MonoBehaviour
    {
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _damage;
        [SerializeField] private float _size;

        private void OnTriggerEnter(Collider other)
        {
            if (1 << other.gameObject.layer != EntityLayers.Enemy)
            {
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, EntityLayers.Enemy, QueryTriggerInteraction.Collide);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_damage);
                    Destroy(gameObject);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}
