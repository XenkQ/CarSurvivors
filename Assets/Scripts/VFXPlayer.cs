using System;
using UnityEngine;

public interface IVFXPlayer
{
    public void Play();

    public event EventHandler OnVFXFinished;
}

public class VFXPlayer : MonoBehaviour, IVFXPlayer
{
    [SerializeField] private float _delayBetweenParticlesFinishedPlayingChecks = 0.1f;

    public event EventHandler OnVFXFinished;

    private ParticleSystem[] _particleSystems;

    private bool _particlesStartedPlaying;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AllParticlesFinished), _delayBetweenParticlesFinishedPlayingChecks, _delayBetweenParticlesFinishedPlayingChecks);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(AllParticlesFinished));
    }

    private void AllParticlesFinished()
    {
        if (!_particlesStartedPlaying)
        {
            return;
        }

        foreach (var particleSystem in _particleSystems)
        {
            if (particleSystem != null && !particleSystem.isPlaying)
            {
                OnVFXFinished?.Invoke(this, EventArgs.Empty);
                _particlesStartedPlaying = false;
                return;
            }
        }
    }

    public void Play()
    {
        foreach (var particleSystem in _particleSystems)
        {
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }

        _particlesStartedPlaying = true;
    }
}
