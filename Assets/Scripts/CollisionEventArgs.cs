using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class CollisionEventArgs : EventArgs
    {
        public Collider Collider { get; set; }

        public CollisionEventArgs(Collider collider)
        {
            Collider = collider;
        }
    }
}
