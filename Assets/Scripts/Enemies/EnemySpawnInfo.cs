using System;

namespace Assets.Scripts.Enemies
{
    [Serializable]
    public class EnemySpawnInfo
    {
        public Enemy EnemyPrefab;
        public ushort MaxAmount;
        public SpawnChanceInfo SpawnChanceInfo;
    }
}
