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
            ServiceContainer.ForCurrentScene().Get(out TalentsService talentsService);
            TalentData data = talentsService.GetTalentData(Setup.TalentType);
            
            return data != default ? 
                GameConditionStatus.Failure : 
                GameConditionStatus.Success;
        }
    }
}