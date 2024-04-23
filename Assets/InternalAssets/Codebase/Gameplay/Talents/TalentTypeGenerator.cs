using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Library.Collections;
using InternalAssets.Codebase.Library.GameConditions;

namespace InternalAssets.Codebase.Gameplay.Talents
{
    public class TalentTypeGenerator
    {
        private readonly TalentsService _talentService;
        
        public TalentTypeGenerator(TalentsService talentService)
        {
            _talentService = talentService;
        }

        public async UniTask<List<TalentType>> Generate(int count = 3)
        {
            List<TalentType> currentAppliedTalentTypes = _talentService.GetAppliedTalentTypes();
            List<TalentType> currentAvailableTalentTypes = _talentService.AvailableTalents;
            List<TalentType> outputTalentTypes = new List<TalentType>();
            
            int maxIteration = 15;
            bool searchFromApplied = false;
            
            while (outputTalentTypes.Count < count && maxIteration > 0)
            {
                GenerateOperationArgs args = await GetTalent(searchFromApplied == false ?
                    currentAppliedTalentTypes :
                    currentAvailableTalentTypes
                );

                if (args.Status)
                {
                    currentAvailableTalentTypes.Remove(args.TalentType);
                    outputTalentTypes.Add(args.TalentType);
                }

                searchFromApplied = true;
                maxIteration--;
            }

            return outputTalentTypes;
        }

        private UniTask<GenerateOperationArgs> GetTalent(List<TalentType> types)
        {
            GenerateOperationArgs args = new GenerateOperationArgs();
            
            if (types.Count <= 0)
                return new UniTask<GenerateOperationArgs>(args);
            
            types.Shuffle();

            for (int i = 0; i < types.Count; i++)
            {
                if (_talentService.GetTalentSetup(types[i]).PickActions.Resolve())
                {
                    args.TalentType = types[i];
                    args.Status = true;
                    
                    break;
                }
            }

            return new UniTask<GenerateOperationArgs>(args);
        }
    }

    public class GenerateOperationArgs
    {
        public TalentType TalentType = TalentType.none;
        public bool Status = false;
    }
}