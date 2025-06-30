using Assets.Scripts.LayerMasks;
using Assets.Scripts.StatusAffectables;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(EnemyMovementController), typeof(EnemyCollisionsController))]
    public class EnemyStuckInsideOtherEnemyPreventer : MonoBehaviour
    {
        [SerializeField] private float separationRadius = 1.2f;
        [SerializeField] private float separationStrength = 0.5f;

        private IMovementController _enemyMovement;

        private Collider _selfCollider;

        private void Awake()
        {
            _enemyMovement = GetComponent<IMovementController>();
            _selfCollider = GetComponent<Collider>();
        }

        private void Update()
        {
            Vector3 separation = Vector3.zero;
            int neighborCount = 0;

            Collider[] hits = Physics.OverlapSphere(transform.position, separationRadius, EntityLayers.Enemy);
            foreach (var hit in hits)
            {
                if (hit == _selfCollider) continue;

                Vector3 away = transform.position - hit.transform.position;
                away.y = 0f;

                float distance = away.magnitude;
                if (distance > 0)
                {
                    separation += away.normalized / distance;
                    neighborCount++;
                }
            }

            if (neighborCount > 0)
            {
                separation /= neighborCount;
                separation = separation.normalized * separationStrength;
            }
            else
            {
                separation = Vector3.zero;
            }

            separation.y = 0f;

            _enemyMovement.SetSeparationVector(separation);
        }
    }
}
