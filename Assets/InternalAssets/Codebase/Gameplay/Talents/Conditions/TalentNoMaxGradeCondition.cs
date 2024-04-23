using ACS.Core.ServicesContainer;
using InternalAssets.Codebase.Library.GameConditions;
using Sirenix.Serialization;

namespace InternalAssets.Codebase.Gameplay.Talents.Conditions
{
    public class TalentNoMaxGradeCondition : IGameCondition
    {
        [OdinSerialize] public TalentSetup Setup;
        
        public GameConditionStatus IsValid()
        {
            ServiceContainer.ForCurrentScene().Get(out TalentsService talentsService);
            TalentData data = talentsService.GetTalentData(Setup.TalentType);
            
            if (data == default)
                return GameConditionStatus.Success;
            
            return data.CurrentGrade.Level >= Setup.Grades.Count ? 
                GameConditionStatus.Failure : 
                GameConditionStatus.Success;
        }
    }
}