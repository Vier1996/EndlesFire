using System.Collections.Generic;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.SkillsTree.Nodes
{
    public class SkillNode : WrappedSkillNode
    {
        [field: SerializeField, Input] public List<SkillNode> Entry { get; private set; } = new();
        [field: SerializeField, Output] public List<SkillNode> Exit { get; private set; } = new();
    }
}
