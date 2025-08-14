using Assets.Scripts.Enemies;
using Assets.Scripts.Spawners.GridSpace;
using Assets.Scripts.UI;
using Reflex.Core;
using UnityEngine;

namespace Assets.Scripts.ReflexDI
{
    public class DefaultGameplaySceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private EnemiesSpawner _enemiesSpawner;
        [SerializeField] private PlayerDeathPresenter _playerDeathPresenter;
        [SerializeField] private TimerPresenter _timerPresenter;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.AddSingleton(_enemiesSpawner, typeof(IOnRandomGridPosSpawner<EnemiesSpawner>));
            builder.AddSingleton(_playerDeathPresenter, typeof(IPlayerDeathPresenter));
            builder.AddSingleton(_timerPresenter, typeof(ITimerPresenter));
        }
    }
}
