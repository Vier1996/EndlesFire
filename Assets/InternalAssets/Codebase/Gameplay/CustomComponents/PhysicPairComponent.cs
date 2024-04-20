using System;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.CustomComponents
{
    [Serializable]
    public class PhysicPairComponent
    {
        [field: SerializeField] public Collider2D Collider;
        [field: SerializeField] public Rigidbody2D Rigidbody;
    }
}
