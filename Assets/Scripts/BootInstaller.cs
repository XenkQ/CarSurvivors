using Assets.Scripts.Audio;
using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class BootLoader : MonoBehaviour
    {
        private void Start()
        {
            void InstallExtra(Scene scene, ContainerBuilder builder)
            {
                builder.AddSingleton(FindFirstObjectByType<AudioMixersManager>(), typeof(IAudioMixersManager));
            }

            SceneScope.OnSceneContainerBuilding += InstallExtra;

            GameScenesAttacher.AttachNewSceneAsync(GameScene.MainMenu);
        }
    }
}