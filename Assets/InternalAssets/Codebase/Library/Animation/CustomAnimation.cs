using UnityEngine;

namespace Codebase.Library.Animation
{
    public static class CustomAnimation
    {
        public static Vector3 GetArcPosition(Vector3 start, Vector3 end, float height, float progress)
        {
            Vector3 targetPosition = Vector3.Lerp(start, end, progress);
            float arc = height * (progress - (progress * progress));

            return new Vector3(targetPosition.x, targetPosition.y + arc, targetPosition.z);
        }
    }
}