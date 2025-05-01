using UnityEngine;

namespace LayerMasks
{
    public static class EntityLayers
    {
        private const string ENEMY = "Enemy";
        private const string PLAYER = "Player";

        public static readonly LayerMask Enemy = LayerMask.GetMask(ENEMY);
        public static readonly LayerMask Player = LayerMask.GetMask(PLAYER);
        public static readonly LayerMask All = LayerMask.GetMask(ENEMY, PLAYER);
    }
}
