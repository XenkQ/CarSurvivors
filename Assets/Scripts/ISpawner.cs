using System;

namespace Assets.Scripts
{
    public interface ISpawner
    {
        public event EventHandler OnSpawnedEntityReleased;
    }
}
