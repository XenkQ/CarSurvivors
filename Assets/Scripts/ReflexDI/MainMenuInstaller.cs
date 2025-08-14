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
                typeof(ISetting<AudioVolumeSetting, float>),
                typeof(ISettingLoader)
            );

            containerBuilder.AddScoped(
                typeof(GraphicSetting),
                typeof(ISetting<GraphicSetting, string>),
                typeof(ISettingLoader)
            );

            containerBuilder.AddScoped(
                typeof(FullScreenSetting),
                typeof(ISetting<FullScreenSetting, FullScreenMode>),
                typeof(ISettingLoader)
            );

            containerBuilder.AddScoped(
                typeof(ResolutionSetting),
                typeof(ISetting<ResolutionSetting, SerializableResolution>),
                typeof(ISettingLoader)
            );

            containerBuilder.AddScoped(
                typeof(DamageNumbersSetting),
                typeof(ISetting<DamageNumbersSetting, bool>),
                typeof(ISettingLoader)
            );
        }
    }
}
