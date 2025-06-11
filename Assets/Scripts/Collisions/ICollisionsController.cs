using Assets.Scripts.Enemies;
using System;

namespace Assets.Scripts
{
    public interface ICollisionsController
    {
        event EventHandler<CollisionEventArgs> OnCollisionWithOtherEnemy;

        event EventHandler<CollisionEventArgs> OnCollisionWithPlayer;
    }
}
