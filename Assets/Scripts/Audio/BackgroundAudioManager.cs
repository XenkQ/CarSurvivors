using Assets.Scripts.Player;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundAudioManager : MonoBehaviour
    {
        [Serializable]
        public class AudioClipInSceneConfig
        {
            public AudioClipConfig ClipConfig;
            public Scenes Scene;
        }

        [SerializeField] private AudioClipInSceneConfig[] _clipConfigInScenes;

        public static BackgroundAudioManager Instance;

        private AudioSource _audioSource;
        private float _deathAudioPitch = 0.6f;

        private BackgroundAudioManager()
        { }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManager_SceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
        }

        private void SceneManager_SceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            PlayOrContinuePlayingCorrectSceneBackgroundMusic();

            PlayerManager.Instance.Health.OnNoHealth -= Health_OnNoHealth;
            PlayerManager.Instance.Health.OnNoHealth += Health_OnNoHealth;
        }

        private void Health_OnNoHealth(object sender, EventArgs e)
        {
            _audioSource.pitch = _deathAudioPitch;
        }

        private void PlayOrContinuePlayingCorrectSceneBackgroundMusic()
        {
            AudioClipConfig clipConfig = _clipConfigInScenes
                .FirstOrDefault(config => config.Scene == SceneLoader.CurrentScene)
                ?.ClipConfig;

            if (clipConfig is null)
            {
                return;
            }

            _audioSource.loop = clipConfig.Loop;
            _audioSource.pitch = clipConfig.Pitch;
            _audioSource.volume = clipConfig.Volume;

            if (clipConfig.AudioClip != _audioSource.clip)
            {
                _audioSource.clip = clipConfig.AudioClip;
                _audioSource.Play();
            }
        }
    }
}
