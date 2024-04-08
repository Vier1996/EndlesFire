using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons
{
    public static class WeaponDispersion
    {
        public static Vector3 Disperse(Vector3 sourceVector, float dispersionRange)
        {
            float randomRotationAngle = Random.Range(-dispersionRange / 2, dispersionRange / 2);
            
            return (Quaternion.AngleAxis(randomRotationAngle, Vector3.forward) * sourceVector) * Random.Range(4.5f, 6f);
        }
    }
}
