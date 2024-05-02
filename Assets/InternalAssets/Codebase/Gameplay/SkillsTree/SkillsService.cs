using System;
using System.Linq;
using ACS.Core.ServicesContainer;
using ACS.Data.DataService.Service;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.SkillsTree.Data;
using InternalAssets.Codebase.Gameplay.SkillsTree.Nodes;

namespace InternalAssets.Codebase.Gameplay.SkillsTree
{
    public class SkillsService : IDisposable
    {
        private readonly IDataService _dataService;
        private SkillsProgressModel _skillsProgressModel;
        
        public SkillsService()
        {
            ServiceContainer.Core.Get(out _dataService);
            
            OnDataChanged();
            
            _dataService.ModelsDataChanged += OnDataChanged;
        }
        
        public void Dispose() => _dataService.ModelsDataChanged -= OnDataChanged;

        public bool IsApplied(SkillBranch branch, WrappedSkillNode skillNode)
        {
            return _skillsProgressModel.IsApplied(new SkillModifyPair(branch, skillNode.SkillType));
        }

        public bool IsUnlocked(SkillBranch branch, WrappedSkillNode skillNode)
        {
            SkillNode inheritNode = skillNode as SkillNode;

            if (inheritNode == null)
                throw new InvalidCastException("Can not cast WrappedSkillNode to SkillNode");

            return inheritNode.Entry.All(wrappedSkillNode => IsApplied(branch, wrappedSkillNode));
        }

        public void ApplySkill(SkillBranch branch, WrappedSkillNode skillNode)
        {
            _skillsProgressModel.ApplySkill(new SkillModifyPair(branch, skillNode.SkillType));
        }

        public void RemoveSkill(SkillBranch branch, WrappedSkillNode skillNode)
        {
            _skillsProgressModel.RemoveSkill(new SkillModifyPair(branch, skillNode.SkillType));
        }

        private void OnDataChanged() => _skillsProgressModel = _dataService.Models.Resolve<SkillsProgressModel>();
    }
}