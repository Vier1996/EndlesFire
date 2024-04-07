using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Codebase.Library.Self
{
    public class SelfYoyo : MonoBehaviour
    {
        [SerializeField] private Ease _yoyoMovementEase;
        
        [SerializeField] private bool _randomOffset = false;
        [SerializeField] private bool _randomDuration = false;
        
        [ShowIf(nameof(_randomOffset)), SerializeField] private float _yoyoMovementOffsetMin;
        [ShowIf(nameof(_randomOffset)), SerializeField] private float _yoyoMovementOffsetMax;
        [ShowIf(nameof(_randomDuration)), SerializeField] private float _yoyoMovementDurationMin;
        [ShowIf(nameof(_randomDuration)), SerializeField] private float _yoyoMovementDurationMax;
        
        [HideIf(nameof(_randomOffset)), SerializeField, Min(0)] private float _yoyoMovementOffset;
        [HideIf(nameof(_randomDuration)), SerializeField, Min(0)] private float _yoyoMovementDuration;
        
        [SerializeField] private bool _reversed = false;

        private Transform _selfTransform;

        private void Awake() => _selfTransform = transform;

        private void Start()
        {
            if (_randomOffset)
                _yoyoMovementOffset = UnityEngine.Random.Range(_yoyoMovementOffsetMin, _yoyoMovementOffsetMax);
            
            if (_randomDuration)
                _yoyoMovementDuration = UnityEngine.Random.Range(_yoyoMovementDurationMin, _yoyoMovementDurationMax);
            
            StartAnimation();
        }

        private void StartAnimation()
        {
            Vector3 localPosition = _selfTransform.localPosition;
            _selfTransform
                .DOLocalMove(new Vector3(localPosition.x, localPosition.y + _yoyoMovementOffset * (_reversed ? -1 : 1), localPosition.z), _yoyoMovementDuration)
                .SetEase(_yoyoMovementEase)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}