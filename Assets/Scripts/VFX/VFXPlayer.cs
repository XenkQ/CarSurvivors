using System;
using UnityEngine;

namespace VFX
{
    public interface IVFXPlayer
    {
        public void Play(bool destroyOnEnd = false);

        public void Play(float simulationSpeed = 1f, bool destroyOnEnd = false);

        public float GetLongestParticleDuration();

        public event EventHandler OnVFXFinished;
    }

    public class VFXPlayer : MonoBehaviour, IVFXPlayer
    {
        public event EventHandler OnVFXFinished;

        private ParticleSystem[] _particleSystems;

        private bool _particlesStartedPlaying;

        private float _longestParticleDuration;

        private bool _destroyOnEnd;

        private void Awake()
        {
            _particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
        }

        public float GetLongestParticleDuration()
        {
            return _longestParticleDuration;
        }

        public void Play(bool destroyOnEnd = false)
        {
            _longestParticleDuration = 0;
            foreach (var particleSystem in _particleSystems)
            {
                if (particleSystem is null)
                {
                    continue;
                }

                float currentDuration = particleSystem.main.duration;
                if (particleSystem.main.duration > _longestParticleDuration)
                {
                    _longestParticleDuration = currentDuration;
                }

                _particlesStartedPlaying = true;
                particleSystem.Play();
            }

            _destroyOnEnd = destroyOnEnd;
            if (_particlesStartedPlaying)
            {
                if (IsInvoking(nameof(OnAllParticlesFinished)))
                {
                    CancelInvoke(nameof(OnAllParticlesFinished));
                }

                InvokeRepeating(nameof(OnAllParticlesFinished), _longestParticleDuration, _longestParticleDuration);
            }
            else
            {
                OnAllParticlesFinished();
            }
        }

        public void Play(float simulationSpeed, bool destroyOnEnd = false)
        {
            foreach (var particleSystem in _particleSystems)
            {
                if (particleSystem != null)
                {
                    var main = particleSystem.main;
                    main.simulationSpeed = simulationSpeed;
                }
            }

            Play(destroyOnEnd);
        }

        private void OnAllParticlesFinished()
        {
            if (!_particlesStartedPlaying)
            {
                return;
            }

            OnVFXFinished?.Invoke(this, EventArgs.Empty);
            _particlesStartedPlaying = false;

            if (_destroyOnEnd)
            {
                Destroy(gameObject);
            }
        }
    }
}
