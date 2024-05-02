using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace InternalAssets.Codebase.Gameplay.SkillsTree.Nodes
{
    [CreateAssetMenu(fileName = nameof(SkillsGraph), menuName = "App/Graphs/" + nameof(SkillsGraph))]
    public class SkillsGraph : NodeGraph
    {
        [field: SerializeField, BoxGroup("Ids")] public SkillBranch SkillBranch { get; private set; } = SkillBranch.none;
    }
}