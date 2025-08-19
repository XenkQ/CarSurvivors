using UnityEngine;

namespace Assets.Scripts.Spawners.WorldSpace
{
    public interface IInWorldSpaceSpawner<TSelf, TSpecificConfig> : ISpawnedObjectsCounter, IObjectReleaseNotifier
        where TSelf : IInWorldSpaceSpawner<TSelf, TSpecificConfig>
    {
        public void Spawn(Vector3 pos, TSpecificConfig specificConfig, int count = 1);
    }
}
