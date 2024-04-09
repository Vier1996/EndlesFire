using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Scanning
{
    public class ScanCircleView : MonoBehaviour, IInitializable<ScanCircleView>
    {
        [Range(0f, 1f)] [SerializeField] private float _maxAlphaVisibility = 1f;
        
        private SpriteRenderer _spriteRenderer;
        private Transform _selfTransform;
        
        public ScanCircleView Bootstrapp()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _selfTransform = GetComponent<Transform>();
            
            Hide(0f, 0f);

            return this;
        }

        public void Dispose()
        {
            _selfTransform.KillTween();
            _spriteRenderer.KillTween();
        }

        public void Show(float multiplier, float animateSeconds = 0.5f, float delaySeconds = 0f) => 
            SetVisibility(multiplier, true, animateSeconds, delaySeconds);

        public void Hide(float multiplier, float animateSeconds = 0.5f, float delaySeconds = 1f) => 
            SetVisibility(multiplier, false, animateSeconds, delaySeconds);

        private void SetVisibility(float multiplier, bool isVisible, float animateSeconds, float delaySeconds)
        {
            _selfTransform.KillTween();
            _spriteRenderer.KillTween();

            float radius = multiplier * 0.75f;
            
            _spriteRenderer.DOFade(isVisible ? _maxAlphaVisibility : 0f, animateSeconds).SetDelay(delaySeconds);
            _selfTransform.DOScale(isVisible ? radius : 0f, animateSeconds).SetDelay(delaySeconds);
        }
    }
}
