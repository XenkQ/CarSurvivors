using Assets.Scripts.Audio;
using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.ReflexDI
{
    public class BootLoader : MonoBehaviour
    {
        [Inject] private readonly IGameSceneLoader _gameSceneLoader;

        private AudioMixersManager _audioMixersManager;
        private BackgroundAudioManager _backgroundAudioManager;

        private void Awake()
        {
            _audioMixersManager = FindFirstObjectByType<AudioMixersManager>();
            _backgroundAudioManager = FindFirstObjectByType<BackgroundAudioManager>();
        }

        private void Start()
        {
            SceneScope.OnSceneContainerBuilding += InstallExtra;

            _gameSceneLoader.LoadNewSceneAsync(GameScene.MainMenu);
        }

        private void OnDisable()
        {
            SceneScope.OnSceneContainerBuilding -= InstallExtra;
        }

        private void InstallExtra(Scene scene, ContainerBuilder builder)
        {
            builder.AddSingleton(_audioMixersManager, typeof(IAudioMixersManager));
            builder.AddSingleton(_backgroundAudioManager, typeof(IBackgroundAudioManager));
        }
    }
}