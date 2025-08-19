using Assets.Scripts.Enemies;
using Assets.Scripts.GridSystem;
using Assets.Scripts.LevelSystem.Exp;
using Assets.Scripts.Skills.ObjectsImpactingSkills.Crate;
using Assets.Scripts.Spawners.GridSpace;
using Assets.Scripts.Spawners.WorldSpace;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Death;
using Reflex.Core;
using UnityEngine;

namespace Assets.Scripts.ReflexDI
{
    public class DefaultGameplaySceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private EnemiesSpawner _enemiesSpawner;
        [SerializeField] private PlayerDeathPresenter _playerDeathPresenter;
        [SerializeField] private TimerPresenter _timerPresenter;
        [SerializeField] private ExpParticleSpawner _expParticleSpawner;
        [SerializeField] private CollectibleItemsSpawner _collectibleItemsSpawner;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.AddSingleton(_gridManager, typeof(IGridManager));
            builder.AddSingleton(_enemiesSpawner, typeof(IOnRandomGridPosSpawner<EnemiesSpawner>));
            builder.AddSingleton(_collectibleItemsSpawner, typeof(IOnRandomGridPosSpawner<CollectibleItemsSpawner>));
            builder.AddSingleton(_expParticleSpawner, typeof(IInWorldSpaceSpawner<ExpParticleSpawner, float>));
            builder.AddSingleton(_playerDeathPresenter, typeof(IPlayerDeathPresenter));
            builder.AddSingleton(_timerPresenter, typeof(ITimerPresenter));
        }
    }
}
