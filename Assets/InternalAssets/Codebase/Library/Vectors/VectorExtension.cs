using UnityEngine;

namespace InternalAssets.Codebase.Library.Vectors
{
    public static class VectorExtension
    {
        public static Vector3 GetPointInRadius(this Vector3 from, float minRadius, float maxRadius)
        {
            float randomAngle = Random.Range(0f, 360f);
            float x = from.x + Random.Range(minRadius, maxRadius) * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float y = from.y + Random.Range(minRadius, maxRadius) * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
            
            return new Vector3(x, y, 0);    
        }
        
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

            return Mathf.Sqrt(x * x + y * y + z * z);
        }
    }
}