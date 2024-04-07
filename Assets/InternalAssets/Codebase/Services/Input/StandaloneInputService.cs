using UnityEngine;

namespace InternalAssets.Codebase.Services.Input
{
    public class StandaloneInputService : IInputService
    {
        public Vector2 Axis => GetAxis();
        public float Vertical => Axis.y;
        public float Horizontal => Axis.x;
        public float DragDistance { get; }

        private Vector2 GetAxis() => 
            new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
    }
}