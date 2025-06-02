using Assets.Scripts.LayerMasks;
using System;
using UnityEngine;

namespace Assets.Scripts.Skills.ObjectsImpactingSkills.Crate
{
    public class SkillCrate : MonoBehaviour, ICollectible
    {
        public event EventHandler OnCollected;

        private void OnTriggerEnter(Collider other)
        {
            if (1 << other.gameObject.layer == EntityLayers.Player)
            {
                OnCollected?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);
            }
        }
    }
}
