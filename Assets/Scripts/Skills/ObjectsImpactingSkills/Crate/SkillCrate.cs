using Assets.Scripts.Extensions;
using Assets.Scripts.LayerMasks;
using DG.Tweening;
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
                Destroy(gameObject);
                OnCollected?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
