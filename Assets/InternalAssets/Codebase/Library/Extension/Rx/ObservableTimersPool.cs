using System.Collections.Generic;
using UnityEngine;

namespace Codebase.Library.Extension.Rx
{
    public class ObservableTimersPool
    {
        private readonly List<OptimizedTimerObservable> _busyTimerObservables = new();
        private readonly List<OptimizedTimerObservable> _availableTimerObservables = new();

        public OptimizedTimerObservable SelectAvailable()
        {
            if (_availableTimerObservables.Count <= 0)
            {
                OptimizedTimerObservable timerObservable = new OptimizedTimerObservable();

                timerObservable.Disposed += Release;

                return timerObservable;
            }

            OptimizedTimerObservable availableTimer = _availableTimerObservables[0];
                
            _availableTimerObservables.RemoveAt(0);
            _busyTimerObservables.Add(availableTimer);

            return availableTimer;
        }

        private void Release(OptimizedTimerObservable timerObservable)
        {
            timerObservable.Dispose();
                
            _busyTimerObservables.Remove(timerObservable);
            _availableTimerObservables.Add(timerObservable);
        }
            
        public void CheckListsSize()
        {
            Debug.Log($"Busy:[{_busyTimerObservables.Count}]");
            Debug.Log($"Available:[{_availableTimerObservables.Count}]");
        }
    }
}