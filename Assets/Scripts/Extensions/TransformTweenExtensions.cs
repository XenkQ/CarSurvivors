using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class TransformTweenExtensions
    {
        public static Tween StartGrowShrinkLoopTween(this Transform transform, Vector3 maxScale, float firstIterationTime = 1f)
        {
            return transform
                .DOScale(maxScale, firstIterationTime)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public static Tween StartRotateLoopTween(this Transform transform, Vector3 maxRotation, float firstIterationTime = 1f)
        {
            return transform
                .DORotate(maxRotation, firstIterationTime, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }

        public static Tween StartLocalRotateLoopTween(this Transform transform, Vector3 maxRotation, float firstIterationTime = 1f)
        {
            return transform
                .DOLocalRotate(maxRotation, firstIterationTime, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }

        public static Tween StartYAngleLocalRotationLoopTween(this Transform transform, float angle, float firstIterationTime = 1f, bool rotationFromEnd = false)
        {
            return transform
                .DOLocalRotate(new Vector3(0, (rotationFromEnd ? -angle : angle) * 0.5f, 0), firstIterationTime)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public static Tween LifeEndingShrinkToZeroTween(this Transform transform, float disapearingShrinkDuration, TweenCallback callback)
        {
            DOTween.Kill(transform);

            return transform.DOScale(Vector3.zero, disapearingShrinkDuration)
                .SetEase(Ease.Flash)
                .OnComplete(callback);
        }
    }
}
