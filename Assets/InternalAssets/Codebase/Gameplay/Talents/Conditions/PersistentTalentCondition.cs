using System;
using ACS.Core.ServicesContainer;
using InternalAssets.Codebase.Library.GameConditions;
using Sirenix.Serialization;

namespace InternalAssets.Codebase.Gameplay.Talents.Conditions
{
    public class PersistentTalentCondition : IGameCondition
    {
        [OdinSerialize] public TalentSetup Setup;
        
        public GameConditionStatus IsValid()
        {
            if (ServiceContainer.ForCurrentScene().TryGetService(out TalentsService talentsService) == false)
                throw new Exception("Can not get TalentsService from local container");
            
            TalentData data = talentsService.GetTalentData(Setup.TalentType);
            
            return data != default ? 
                GameConditionStatus.Failure : 
                GameConditionStatus.Success;
        }
    }
}