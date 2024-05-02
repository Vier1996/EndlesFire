using System;
using System.Collections.Generic;
using ACS.Data.DataService.Model;
using InternalAssets.Codebase.Gameplay.Enums;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace InternalAssets.Codebase.Gameplay.SkillsTree.Data
{
    [Preserve]
    public class SkillsProgressModel : ProgressModel
    {
        [JsonProperty] private Dictionary<SkillBranch, SkillBranchProgressData> _branchProgressData = new();

        public bool IsApplied(SkillModifyPair pair)
        {
            if (_branchProgressData.ContainsKey(pair.Branch) == false) return false;

            SkillBranchProgressData branchData = _branchProgressData[pair.Branch];
            
            return branchData.Get(pair.SkillType);
        }

        public void ApplySkill(SkillModifyPair pair)
        {
            SkillBranchProgressData branchData = GetBranchData(pair.Branch);
            
            branchData.Set(pair.SkillType, true);
        }

        public void RemoveSkill(SkillModifyPair pair)
        {
            SkillBranchProgressData branchData = GetBranchData(pair.Branch);

            branchData.Set(pair.SkillType, false);
        }

        private SkillBranchProgressData GetBranchData(SkillBranch branchType)
        {
            SkillBranchProgressData branchData = null;

            if (_branchProgressData.TryAdd(branchType, branchData = new SkillBranchProgressData()) == false)
                branchData = _branchProgressData[branchType];
                    
            return branchData;
        }
    }

    [Serializable]
    public class SkillBranchProgressData
    {
        private Dictionary<SkillType, bool> _skillsData = new();
        
        public bool Get(SkillType skillType) => _skillsData.ContainsKey(skillType) && _skillsData[skillType];

        public void Set(SkillType skillType, bool value)
        {
            _skillsData.TryAdd(skillType, false);
            _skillsData[skillType] = value;
        }
    }

    public class SkillModifyPair
    {
        public SkillBranch Branch { get; private set; }
        public SkillType SkillType { get; private set; }

        public SkillModifyPair(SkillBranch branch, SkillType skillType)
        {
            Branch = branch;
            SkillType = skillType;
        }
    }
}