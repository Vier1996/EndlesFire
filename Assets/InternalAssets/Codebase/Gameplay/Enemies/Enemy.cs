using Codebase.Library.SAD;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Enemies
{
    public abstract class Enemy : Entity, IEnemy, ITargetable
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