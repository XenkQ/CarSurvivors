using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public interface IAudioClipPlayer
    {
        public void Play(string name);

        public event EventHandler OnAudioClipFinished;
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioClipPlayer : MonoBehaviour, IAudioClipPlayer
    {
        [Serializable]
        public class AudioClipPlayerConfig
        {
            public string Name;
            public AudioClipConfig[] ClipVariants;
        }

        [SerializeField] private AudioClipPlayerConfig[] _audioClipPlayerConfigs;

        public event EventHandler OnAudioClipFinished;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Play(string name)
        {
            AudioClipPlayerConfig config = _audioClipPlayerConfigs.FirstOrDefault(c => c.Name == name);

            if (config is null && config.ClipVariants.Length == 0)
            {
                return;
            }

            AudioClipConfig clipConfigVariant =
                config.ClipVariants[UnityEngine.Random.Range(0, config.ClipVariants.Length)];

            PrepareAudioSourceToPlayClip(clipConfigVariant);

            if (IsInvoking(nameof(OnAudioClipPlayFinished)))
            {
                CancelInvoke(nameof(OnAudioClipPlayFinished));
            }

            _audioSource.Play();

            Invoke(nameof(OnAudioClipPlayFinished), _audioSource.clip.length);
        }

        private void OnAudioClipPlayFinished()
        {
            OnAudioClipFinished?.Invoke(this, EventArgs.Empty);
        }

        private void PrepareAudioSourceToPlayClip(AudioClipConfig config)
        {
            _audioSource.clip = config.AudioClip;
            _audioSource.volume = config.Volume;
            _audioSource.pitch = config.Pitch;
            _audioSource.loop = config.Loop;
        }
    }
}
