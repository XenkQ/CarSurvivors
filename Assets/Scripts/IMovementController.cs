using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IMovementController
    {
        public float GetCurrentMovementSpeed();

        public void SetSeparationVector(Vector3 separation);

        public Tween MoveToPositionInTimeIgnoringSpeed(Vector3 pos, float time);
    }
}
