using UnityEngine;

namespace InternalAssets.Codebase.Services.Input
{
    public interface IInputService
    {
        Vector2 Axis { get; }
        float Vertical { get; }
        float Horizontal { get; }
        
        public float DragDistance { get; }
    }
}