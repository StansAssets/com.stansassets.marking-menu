using UnityEngine;

namespace StansAssets.MarkingMenu
{
    // TODO: check a usefulness of this code. Seems like redundant
    static class GeometryUtility
    {
        internal static float GetTrigonometricalAngle(Vector2 center, Vector2 pointOnCircle)
        {
            Vector2 v = (pointOnCircle - center).normalized;
            return Mathf.Atan(v.x / v.y) * Mathf.Rad2Deg;
        }
    }
}
