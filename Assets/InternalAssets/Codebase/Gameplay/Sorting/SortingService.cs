using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Codebase.Gameplay.Sorting
{
    public class SortingService : IDisposable
    {
        private const float UPDATE_TIME = 0.04f;
        
        private IDisposable _fixedUpdateDisposable;
        private SortingParameters _parameters;
        private SortingConfig _sortingConfig;

        private float _updateTimer = 0f; 
        
        private List<ISortableItem> _subscribers = new List<ISortableItem>();
        
        public SortingService()
        {
            _sortingConfig = SortingConfig.GetInstance();
            
            _parameters = new SortingParameters()
            {
                WorkingLayerName = _sortingConfig.WorkingLayerName,
                LowerLimit = -_sortingConfig.LimitOffset,
                UpperLimit = _sortingConfig.LimitOffset,
                LimitsSum = _sortingConfig.LimitOffset * 2,
                Offset = _sortingConfig.LimitOffset,
                SortingSensitivityOrderValue = (_sortingConfig.LimitOffset * 2) / _sortingConfig.SortingSensitivity,
            };

            _updateTimer = UPDATE_TIME;
            _fixedUpdateDisposable = Observable.EveryUpdate().Subscribe(_ => UpdateSubscribeOrders());
        }

        public SortingParameters Subscribe(ISortableItem subscriber)
        {
            if (!_subscribers.Contains(subscriber)) 
                _subscribers.Add(subscriber);

            return _parameters;
        }
        
        public void Unsubscribe(ISortableItem subscriber)
        {
            if (_subscribers.Contains(subscriber)) 
                _subscribers.Remove(subscriber);
        }

        private void UpdateSubscribeOrders()
        {
            if (_updateTimer > 0)
            {
                _updateTimer -= Time.deltaTime;
                return;
            }

            _updateTimer = UPDATE_TIME;
            
            for (int i = 0; i < _subscribers.Count; i++)
            {
                ISortableItem sortableItem = _subscribers[i];
                
                if(sortableItem == null)
                    continue;
                
                if (!sortableItem.IsAnchorValid())
                {
                    Unsubscribe(sortableItem);
                    continue;
                }
                
                sortableItem.UpdateOrder();
            }
        }

        public void Dispose()
        {
            _fixedUpdateDisposable?.Dispose();
        }
        
        [Serializable]
        public class SortingParameters
        {
            public string WorkingLayerName;
            public float LowerLimit;
            public float UpperLimit;
            public float LimitsSum;
            public float Offset;
            public float SortingSensitivityOrderValue;
        }
    }
}