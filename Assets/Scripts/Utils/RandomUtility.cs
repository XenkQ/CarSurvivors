using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class RandomUtility
    {
        public static Vector3 GetRandomPositionFromCircle(Vector3 center, float radius)
        {
            return center + (Random.insideUnitSphere * radius);
        }
    }
}
