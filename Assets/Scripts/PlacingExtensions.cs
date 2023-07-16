using UnityEngine;

public static class PlacingExtensions
{
    public static bool InsideRects(this Vector3 vector3, Rect[] rects)
    {
        Vector2 vector2 = new Vector2(vector3.x, vector3.z);
        foreach (var rect in rects)
        {
            if (rect.Contains(vector2))
            {
                return true;
            }
        }
        return false;
    }
}