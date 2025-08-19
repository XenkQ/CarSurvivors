using System;

namespace Assets.Scripts
{
    public interface IObjectReleaseNotifier
    {
        event EventHandler OnSpawnedEntityReleased;
    }
}
