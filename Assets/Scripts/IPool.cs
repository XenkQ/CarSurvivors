using System;

namespace Assets.Scripts
{
    public interface IPool
    {
        event EventHandler OnSpawnedEntityReleased;
    }
}
