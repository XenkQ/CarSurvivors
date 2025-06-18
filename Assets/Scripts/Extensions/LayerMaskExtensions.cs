using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class LayerMaskExtensions
    {
        public static LayerMask LayerToMask(this LayerMask _, int layer)
        {
            return LayerMask.GetMask(LayerMask.LayerToName(layer));
        }
    }
}
