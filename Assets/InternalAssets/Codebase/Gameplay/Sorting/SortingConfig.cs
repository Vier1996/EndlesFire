using Codebase.Library.Extension.ScriptableObject;
using UnityEngine;

namespace Codebase.Gameplay.Sorting
{
    [CreateAssetMenu(fileName = "SortingConfig", menuName = "App/Configs/SortingConfig")]
    public class SortingConfig : LoadableScriptableObject<SortingConfig> 
    {
        [field: SerializeField] public string WorkingLayerName {get; private set; } = "Default";
        [field: SerializeField] public float SortingSensitivity {get; private set; } = 0.1f;
        [field: SerializeField] public float LimitOffset {get; private set; } = 100f;
    }
}