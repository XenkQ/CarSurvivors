using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public static class TransformTweenExtensions
    {
        public static Tween StartGrowShrinkLoopTween(this Transform transform, Vector3 maxScale, float firstIterationTime = 1f)
        {
            return transform
                .DOScale(maxScale, firstIterationTime)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public static Tween StartXY360RotateLoopTween(this Transform transform, Vector3 maxRotation, float firstIterationTime = 1f)
        {
            return transform
                .DORotate(maxRotation, firstIterationTime, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }
}
