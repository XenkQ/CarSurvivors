using System;

namespace Assets.Scripts.Collisions
{
    public interface ICollisionsController
    {
        public event EventHandler<CollisionEventArgs> OnCollisionWithOtherEnemy;

        public event EventHandler<CollisionEventArgs> OnCollisionWithPlayer;
    }
}
