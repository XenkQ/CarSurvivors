using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Resolution;
using Reflex.Core;
using UnityEngine;

namespace Assets.Scripts.ReflexDI
{
    public class MainMenuInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            //Settings
            containerBuilder.AddScoped(
                typeof(AudioVolumeSetting),
                typeof(AudioVolumeSetting),
                typeof(ISettingLoader)
            );

            containerBuilder.AddScoped(
                typeof(GraphicSetting),
                typeof(GraphicSetting),
                typeof(ISettingLoader)
            );

            containerBuilder.AddScoped(
                typeof(FullScreenSetting),
                typeof(FullScreenSetting),
                typeof(ISettingLoader)
            );

            containerBuilder.AddScoped(
                typeof(ResolutionSetting),
                typeof(ResolutionSetting),
                typeof(ISettingLoader)
            );
        }
    }
}