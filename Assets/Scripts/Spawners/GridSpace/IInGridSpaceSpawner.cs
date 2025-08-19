using Assets.Scripts.GridSystem;

namespace Assets.Scripts.Spawners.GridSpace
{
    public interface IInGridSpaceSpawner<TSelf, TSpecificConfig> : ISpawnedObjectsCounter, IObjectReleaseNotifier
        where TSelf : IInGridSpaceSpawner<TSelf, TSpecificConfig>
    {
        public void Spawn(Cell cell, TSpecificConfig specificConfig, int count = 1);
    }
}
