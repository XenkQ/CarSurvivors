using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class DeathVolume : MonoBehaviour
    {
        [SerializeField] private BoxCollider _boxCollider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeFullHpDamage();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(transform.position, _boxCollider.size);
        }
    }
}
