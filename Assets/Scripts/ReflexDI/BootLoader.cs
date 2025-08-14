using Assets.Scripts.Audio;
using Assets.Scripts.DamageNumbers;
using Assets.Scripts.Settings;
using Assets.Scripts.Spawners.WorldSpace;
using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.ReflexDI
{
    public class BootLoader : MonoBehaviour
    {
        [Inject] private readonly IGameSceneLoader _gameSceneLoader;

        [SerializeField] private AudioMixersManager _audioMixersManager;
        [SerializeField] private BackgroundAudioManager _backgroundAudioManager;
        [SerializeField] private DamageNumbersSpawner _damageNumbersSpawner;

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
            builder.AddSingleton(
                _damageNumbersSpawner,
                typeof(IInWorldSpaceSpawner<DamageNumbersSpawner, DamageNubmersSpawnerConfig>),
                typeof(IEnableDisableFunctionalityTrigger<DamageNumbersSpawner>)
            );
        }
    }
}
