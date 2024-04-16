using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InternalAssets.Codebase.Gameplay.Values
{
    [Serializable]
    public class ValueConfig
    {
        [SerializeField] private bool _isConstant = true;
        
        [SerializeField, Min(0), HideIf(nameof(_isConstant))] private long _minCount = 0;
        [SerializeField, Min(long.MaxValue), HideIf(nameof(_isConstant))] private long _maxCount = 0;
        [SerializeField, ShowIf(nameof(_isConstant))] private long _constantCount = 0;

        public long Count => _isConstant ? _constantCount : ((long)Random.Range(_minCount, _maxCount));
    }
}