using UnityEngine;

public static class DebugUtilities
{
    public static void DrawCircle(Vector3 center, float radius, int segments = 32, Color? color = null, float duration = 0f)
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
}
