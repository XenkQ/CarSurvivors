using Assets.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    [Serializable]
    public class ProjectileSpawnConfig
    {
        public ProjectileConfigSO ProjectileConfigSO;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 MovementDirection;
    }
}
