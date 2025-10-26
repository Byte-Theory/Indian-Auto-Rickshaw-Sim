using UnityEngine;

public static class MathUtils
{
    public static Vector3 GetParabolaPoint(Vector3 start, Vector3 end, float height, float t)
    {
        Vector3 linearPoint = Vector3.Lerp(start, end, t);

        float parabola = -4f * height * (t - 0.5f) * (t - 0.5f) + height;
        linearPoint.y += parabola;

        return linearPoint;
    }
}
