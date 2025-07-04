using System;

namespace Assets.Scripts.Spawners
{
    public interface ISpawner
    {
        public event EventHandler OnSpawnedEntityReleased;
    }
}
