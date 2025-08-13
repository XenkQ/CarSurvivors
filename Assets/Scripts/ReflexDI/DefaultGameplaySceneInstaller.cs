using Assets.Scripts.DamageNumbers;
using Assets.Scripts.Enemies;
using Assets.Scripts.Spawners.GridSpace;
using Assets.Scripts.Spawners.WorldSpace;
using Reflex.Core;
using UnityEngine;

namespace Assets.Scripts.ReflexDI
{
    public class DefaultGameplaySceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private EnemiesSpawner _enemiesSpawner;
        [SerializeField] private DamageNumbersSpawner _damageNumbersSpawner;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(_enemiesSpawner, typeof(IOnRandomGridPosSpawner<EnemiesSpawner>));
            containerBuilder.AddSingleton(
                _damageNumbersSpawner,
                typeof(IInWorldSpaceSpawner<DamageNumbersSpawner, DamageNubmersSpawnerConfig>),
                typeof(IEnableDisableFunctionalityTrigger<DamageNumbersSpawner>)
            );
        }
    }
}
