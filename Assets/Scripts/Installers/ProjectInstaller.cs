using Assets.Scripts.Audio;
using Reflex.Core;
using UnityEngine;

namespace Assets.Scripts.Installers
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(typeof(AudioMixersManager), typeof(IAudioMixersManager));
        }
    }
}