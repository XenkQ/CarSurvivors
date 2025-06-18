using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IMovementController
    {
        public float GetCurrentMovementSpeed();

        public Vector3? MoveToPosition(Vector3 pos);

        public Tween MoveToPositionInTimeIgnoringSpeed(Vector3 pos, float time);
    }
}
