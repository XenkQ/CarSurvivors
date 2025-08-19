using Assets.Scripts.Pooling;

namespace Assets.Scripts.Spawners.GridSpace
{
    public interface IOnRandomGridPosSpawner<TSelf> : ISpawnedObjectsCounter, IObjectReleaseNotifier
        where TSelf : IOnRandomGridPosSpawner<TSelf>
    {
        public void SpawnAtRandomGridPos(int count = 1);
    }
}
