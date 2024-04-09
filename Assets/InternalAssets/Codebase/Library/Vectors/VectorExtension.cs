using UnityEngine;

namespace InternalAssets.Codebase.Library.Vectors
{
    public static class VectorExtension
    {
        public static float DistanceXY(this Vector3 from, Vector3 to)
        {
            return CalculateDistance(to.x, from.x, to.y, from.y);
        }
        
        public static float DistanceXZ(this Vector3 from, Vector3 to)
        {
            return CalculateDistance(to.x, from.x, to.z, from.z);
        }
        
        public static float DistanceYZ(this Vector3 from, Vector3 to)
        {
            return CalculateDistance(to.y, from.y, to.z, from.z);
        }
        
        public static float DistanceXYZ(this Vector3 from, Vector3 to)
        {
            return CalculateDistance(to.x, from.x, to.y, from.y, to.z, from.z);
        }

        private static float CalculateDistance(float vX1, float vX2, float vY1, float vY2, float vZ1 = 0, float vZ2 = 0)
        {
            float x = vX2 - vX1;
            float y = vY2 - vY1;
            float z = vZ2 - vZ1;

            return x * x + y * y + z * z;
        }
    }
}