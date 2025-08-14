using Assets.Scripts.Storage;

namespace Assets.Scripts.Settings
{
    public interface ISetting<TSelf, TRepresentedBy> : IAppStorageValue<TRepresentedBy>, ISettingLoader
        where TSelf : ISetting<TSelf, TRepresentedBy>
    {
    }
}
