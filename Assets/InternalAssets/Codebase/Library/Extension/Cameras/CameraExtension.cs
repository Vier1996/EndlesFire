
using UnityEngine;

namespace InternalAssets.Codebase.Library.Extension.Cameras
{
    public static class CameraExtension
    {
        public static bool IsPointOffScreen(this UnityEngine.Camera camera, Vector3 point, float spawnOffset = 0.5f)
        {
            Vector3 screenPoint = camera.WorldToViewportPoint(point);
            
            return screenPoint.x < -spawnOffset || screenPoint.x > 1 + spawnOffset ||
                   screenPoint.y < -spawnOffset || screenPoint.y > 1 + spawnOffset ||
                   screenPoint.z < 0;
        }
    }
}