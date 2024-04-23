using System;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Codebase.Gameplay.Enums;

namespace InternalAssets.Codebase.Gameplay.Talents
{
    public class TalentsService : IDisposable
    {
        private readonly TalentsConfig _talentsConfig;
        private readonly List<TalentData> _talentData = new();

        public TalentsService() => _talentsConfig = TalentsConfig.GetInstance();

        public void Dispose()
        {
            _talentData.ForEach(td => td.Dispose());
            _talentData.Clear();
        }

        public void ApplyTalent(TalentType talentType)
        {
            TalentData data = TryInsert(talentType);
            
            int nextLevel = (data.CurrentGrade?.Level ?? 0) + 1;
            
            data.CurrentGrade = GetGrade(talentType, nextLevel);
            data.NotifySubscribers();
        }

        public void Subscribe(TalentType talentType, Action<TalentGrade> subscriber) => TryInsert(talentType).AddSubscriber(subscriber);
        public void Unsubscribe(TalentType talentType, Action<TalentGrade> subscriber) => TryInsert(talentType).RemoveSubscriber(subscriber);
        private TalentSetup GetTalentSetup(TalentType talentType) => _talentsConfig.Get(talentType);
        private TalentGrade GetGrade(TalentType talentType, int level) => GetTalentSetup(talentType).GetGrade(level);

        private TalentData TryInsert(TalentType talentType)
        {
            if (_talentData.Any(td => td.TalentType == talentType))
                return _talentData.First(td => td.TalentType == talentType);
            
            TalentData data = new TalentData()
            {
                TalentType = talentType,
                CurrentGrade = null,
            };
                
            _talentData.Add(data);

            return data;

        }
    }
}