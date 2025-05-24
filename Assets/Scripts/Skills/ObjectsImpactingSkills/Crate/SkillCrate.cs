using Assets.Scripts.Extensions;
using Assets.Scripts.LayerMasks;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Skills.ObjectsImpactingSkills.Crate
{
    public class SkillCrate : MonoBehaviour, ICollectible
    {
        private Tween _rotationTween;
        private Vector3 _maxTweenRotation = new Vector3(360, 360, 0);
        private float _tweenIterationTime = 2.5f;

        public event EventHandler OnCollected;

        private void OnEnable()
        {
            if (_rotationTween == null)
            {
                _rotationTween = transform.StartXY360RotateLoopTween(_maxTweenRotation, _tweenIterationTime);
            }
            else
            {
                _rotationTween.Restart();
            }
        }

        private void OnDisable()
        {
            if (_rotationTween != null)
            {
                _rotationTween.Pause();
            }
        }

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
