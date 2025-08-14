using Assets.Scripts.Enemies;
using Assets.Scripts.Spawners.GridSpace;
using Reflex.Core;
using UnityEngine;

namespace Assets.Scripts.ReflexDI
{
    public class DefaultGameplaySceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private EnemiesSpawner _enemiesSpawner;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(_enemiesSpawner, typeof(IOnRandomGridPosSpawner<EnemiesSpawner>));
        }
    }
}
