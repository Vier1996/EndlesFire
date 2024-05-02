using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Extension.ScriptableObject;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.SkillsTree.Nodes;
using InternalAssets.Codebase.Library.ProjectAssets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.SkillsTree
{
    [CreateAssetMenu(fileName = nameof(SkillGraphsContainer), menuName = "App/Configs/Skill/" + nameof(SkillGraphsContainer))]
    public class SkillGraphsContainer : LoadableScriptableObject<SkillGraphsContainer>
    {
        [SerializeField] private List<SkillsGraph> _graphDatas = new();

        public SkillsGraph Get(SkillBranch skillBranch)
        {
            SkillsGraph data = _graphDatas.FirstOrDefault(sgd => sgd.SkillBranch == skillBranch);

            if (data == default)
                throw new ArgumentException($"Can not get graph data with branch type:[{skillBranch}]");

            return data;
        }

#if UNITY_EDITOR
        [Button]
        private void CollectAll()
        {
            _graphDatas.Clear();

            if (AssetsCollector.TryGetAssets(out List<SkillsGraph> list))
                _graphDatas = list;
        }
#endif
    }
}