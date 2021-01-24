using UnityEngine;

namespace Assets.Core.Common
{
    public static class ProjectMath
    {
        public static Vector2 Vector2Rotate(Vector2 point, float angle)
        {
            var x = point.x * Mathf.Cos(angle) - point.y * Mathf.Sin(angle);
            var y = point.x * Mathf.Sin(angle) + point.y * Mathf.Cos(angle);

            return new Vector2(x, y);
        }
    }
}
