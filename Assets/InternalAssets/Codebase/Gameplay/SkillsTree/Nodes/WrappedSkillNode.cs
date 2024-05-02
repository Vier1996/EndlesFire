using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace InternalAssets.Codebase.Gameplay.SkillsTree.Nodes
{
    public class WrappedSkillNode : Node
    {
        [field: SerializeField, BoxGroup("Ids")] public SkillType SkillType { get; private set; } = SkillType.none;
        
        [field: SerializeField, BoxGroup("General")] public string NameKey { get; private set; } = string.Empty;
        [field: SerializeField, BoxGroup("General")] public string DescriptionKey { get; private set; } = string.Empty;
        [field: SerializeField, BoxGroup("General")] public Sprite Icon { get; private set; } = null;
        [field: SerializeField, BoxGroup("Characteristic")] public CharacteristicType CharacteristicType { get; private set; }
        [field: SerializeField, BoxGroup("Characteristic")] public float Value { get; private set; } = 0f;
    }
}