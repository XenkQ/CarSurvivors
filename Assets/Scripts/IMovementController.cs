using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IMovementController
    {
        public float GetCurrentMovementSpeed();

        public bool IsOnGround();

        public Tween MoveToPositionInTimeIgnoringSpeed(Vector3 pos, float time);
    }
}
