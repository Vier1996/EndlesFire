using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Map.Floor
{
    public class RecycledFloorElement : MonoBehaviour
    {
        [field: SerializeField] public GameObject GameObject { get; private set; }
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public SpriteRenderer Renderer { get; private set; }
    }
}