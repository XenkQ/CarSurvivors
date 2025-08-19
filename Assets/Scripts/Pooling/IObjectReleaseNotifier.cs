using System;

namespace Assets.Scripts.Pooling
{
    public interface IObjectReleaseNotifier
    {
        event EventHandler OnSpawnedEntityReleased;
    }
}
