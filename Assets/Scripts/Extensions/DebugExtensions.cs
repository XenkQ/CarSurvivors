using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class DebugExtensions
    {
        public static void DrawCircle(
            this Debug debug,
            Vector3 center,
            float radius,
            int segments = 16,
            Color? color = null,
            float duration = 0f)
        {
            color ??= Color.white;

            float angleStep = 360f / segments;
            Vector3 prevPoint = center + new Vector3(Mathf.Cos(0) * radius, 0, Mathf.Sin(0) * radius);

            for (int i = 1; i <= segments; i++)
            {
                float rad = Mathf.Deg2Rad * angleStep * i;
                Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius);

                Debug.DrawLine(prevPoint, nextPoint, color.Value, duration);
                prevPoint = nextPoint;
            }
        }

        public static void DrawArc(
            this Debug debug,
            Vector3 center,
            Vector3 forward,
            float arcAngle,
            float range,
            int segments = 16,
            Color? color = null)
        {
            color ??= Color.white;

            float halfAngle = arcAngle * 0.5f;

            Vector3 prevPoint = center + Quaternion.Euler(0, -halfAngle, 0) * forward * range;
            for (int i = 1; i <= segments; i++)
            {
                float angle = -halfAngle + (arcAngle * i) / segments;
                Vector3 nextPoint = center + Quaternion.Euler(0, angle, 0) * forward * range;
                Debug.DrawLine(prevPoint, nextPoint, color.Value);
                Debug.DrawLine(center, nextPoint, color.Value);
                prevPoint = nextPoint;
            }
        }
    }
}
