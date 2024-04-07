using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.PlayerFolder
{
    public static class PlayerAnimationProperties
    {
        public static readonly int MoveProperty = Animator.StringToHash("MoveProperty");
        public static readonly int Dodge = Animator.StringToHash("Dodge");
        public static readonly int IdleProperty = Animator.StringToHash("IdleProperty");
    }
}