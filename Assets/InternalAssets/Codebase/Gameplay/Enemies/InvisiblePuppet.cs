using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Enemies
{
    public class InvisiblePuppet : MonoBehaviour, ITargetable
    {
        public Transform GetTargetTransform() => transform;

        public void EnableMarker()
        {
        }

        public void DisableMarker()
        {
        }
    }
}
