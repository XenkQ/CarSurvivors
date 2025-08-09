using Assets.Scripts.Settings;
using Reflex.Core;
using UnityEngine;

namespace Assets.Scripts.ReflexDI
{
    public class MainMenuInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(
                typeof(AudioVolumeSetting),
                typeof(AudioVolumeSetting),
                typeof(ISettingLoader)
            );

            containerBuilder.AddSingleton(
                typeof(GraphicSetting),
                typeof(GraphicSetting),
                typeof(ISettingLoader)
            );
        }
    }
}