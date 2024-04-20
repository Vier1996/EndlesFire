using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Parents
{
    public class SceneAssetParentsContainer : MonoBehaviour
    {
        [field: SerializeField] public Transform PlayerParent { get; private set; }
        [field: SerializeField] public Transform EnemiesParent { get; private set; }
        [field: SerializeField] public Transform ItemsParent { get; private set; }
        [field: SerializeField] public Transform VfxParent { get; private set; }
    }
}
