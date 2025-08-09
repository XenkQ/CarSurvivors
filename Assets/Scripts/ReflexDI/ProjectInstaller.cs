using Reflex.Core;
using UnityEngine;

namespace Assets.Scripts.ReflexDI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(typeof(GameSceneLoader), typeof(IGameSceneLoader));
        }
    }
}