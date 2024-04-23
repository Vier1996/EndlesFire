using System;
using System.Collections.Generic;
using InternalAssets.Codebase.Gameplay.Enums;

namespace InternalAssets.Codebase.Gameplay.Talents
{
    public class TalentData : IDisposable
    {
        public TalentType TalentType = TalentType.none;
        public TalentGrade CurrentGrade = null;
        
        private readonly List<Action<TalentGrade>> _subscribers = new();

        public void AddSubscriber(Action<TalentGrade> subscriber)
        {
            if(_subscribers.Contains(subscriber))
                return;
            
            _subscribers.Add(subscriber);
        }

        public void RemoveSubscriber(Action<TalentGrade> subscriber)
        {
            if(_subscribers.Contains(subscriber) == false)
                return;
            
            _subscribers.Remove(subscriber);
        }

        public void NotifySubscribers() => _subscribers.ForEach(sb => sb.Invoke(CurrentGrade));

        public void Dispose()
        {
            for (int i = 0; i < _subscribers.Count; i++) 
                _subscribers[i] = null;

            _subscribers.Clear();
        }
    }
}