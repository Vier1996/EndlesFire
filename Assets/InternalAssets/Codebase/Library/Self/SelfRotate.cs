using System;
using DG.Tweening;
using UnityEngine;

namespace Codebase.Library.Self
{
    public class SelfRotate : MonoBehaviour
    {
        [SerializeField] private Ease _rotationtEase;
        [SerializeField] private float _offsetRotationt;
        [SerializeField] private float _rotationtDuration;

        private Transform _selfTransform;
        
        private void Start()
        {
            _selfTransform = transform;
            
            StartAnimation();
        }

        private void StartAnimation() =>
            _selfTransform
                .DOLocalRotate(new Vector3(0, 0, _offsetRotationt), _rotationtDuration * 0.5f, RotateMode.Fast)
                .SetEase(_rotationtEase)
                .OnComplete(() =>
                {
                    _selfTransform
                        .DOLocalRotate(new Vector3(0, 0, -_offsetRotationt * 0.5f), _rotationtDuration, RotateMode.Fast)
                        .SetEase(_rotationtEase)
                        .SetLoops(-1, LoopType.Yoyo);
                });
    }
}
