using Assets.Scripts.Settings;
using Reflex.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Installers
{
    public class MainMenuInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
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
        }
    }
}