using System;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.Extension.MonoBehavior;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.ProgressTimers
{
    public class ProgressTimerView : MonoBehaviour, IRecycledClass<ProgressTimerView>
    {
        private static readonly int ProgressShaderProperty = Shader.PropertyToID("_Arc2");

        public event Action ProgressComplete;
        
        [BoxGroup("General"), SerializeField] private Transform _selfTransform;
        [BoxGroup("Params"), SerializeField] private float _maxScale = 1f;
        [BoxGroup("Params"), SerializeField] private float _animationDuration = 1f;
        [BoxGroup("Renderer"), SerializeField] private Renderer _selfRenderer;

        private IDisposable _updateProgressDisposable;
        private Material _progressMaterial;
        
        private float _progress = 0;
        private float _unlockTime = 0f;
        private bool _isIncreased = false;
        private bool _isDisplayed = false;

        private void Awake() => _progressMaterial = _selfRenderer.material;
        
        public ProgressTimerView Enable()
        {
            ResetTimer();
            
            _updateProgressDisposable?.Dispose();
            _updateProgressDisposable = Observable.EveryFixedUpdate().Subscribe(_ => UpdateProgress());
            
            return this;
        }

        public ProgressTimerView Disable()
        {
            _updateProgressDisposable?.Dispose();
            
            TryHide(instant: true);
            
            return this;
        }
        
        public void SetUnlockTime(float unlockTime) => _unlockTime = unlockTime;

        public void BeginInteract() => _isIncreased = true;

        public void StopInteract() => _isIncreased = false;

        public void DisplayTimer(bool instant = false) => TryDisplay(instant: instant);
        public void HideTimer(bool instant = false) => TryHide(instant: instant);
        
        private void ResetTimer()
        {
            _progress = 0;
            _isIncreased = false;
            _isDisplayed = false;

            OnProgressChanged(0f);
            TryHide(instant: true); 
        }

        private void UpdateProgress()
        {
            bool needToUpdateRendererValue = true;
            
            _progress += Time.fixedDeltaTime * (_isIncreased ? 1 : -1f);
            _progress = _progress < 0f ? 0f : _progress;
            
            if (_progress <= 0) needToUpdateRendererValue = TryHide();
            if (_progress > 0) TryDisplay();
            if (_progress >= _unlockTime) OnProgressComplete();
            
            if (needToUpdateRendererValue) 
                OnProgressChanged(_progress / _unlockTime);
        }
        
        private void OnProgressChanged(float progress)
        {
            float value = 360 - (360 * progress);
            
            _progressMaterial.SetFloat(ProgressShaderProperty, value);
        }

        private void OnProgressComplete()
        {
            _updateProgressDisposable?.Dispose();
            
            TryHide(onComplete: () => ProgressComplete?.Invoke());
        }

        private bool TryDisplay(bool instant = false, Action onComplete = null)
        {
            if(instant == false && _isDisplayed)
                return false;

            _isDisplayed = true;
            _selfTransform.KillTween();
            
            if (instant)
            {
                _selfTransform.localScale = Vector3.one * _maxScale;
                
                onComplete?.Invoke();
                
                return true;
            }
            
            _selfTransform.DisplayBubbled(_maxScale * 1.1f, _animationDuration, defaultScale: _maxScale, onComplete: onComplete);
            
            return true;
        }

        private bool TryHide(bool instant = false, Action onComplete = null)
        {
            if(instant == false && _isDisplayed == false)
                return false;
            
            _isDisplayed = false;
            _selfTransform.KillTween();

            if (instant)
            {
                _selfTransform.localScale = Vector3.zero;
                
                onComplete?.Invoke();
                onComplete = null;
                
                return true;
            }
            
            _selfTransform.DisplayBubbled(_maxScale * 1.1f, _animationDuration, defaultScale: 0f, onComplete: onComplete);
            
            return true;
        }
    }
}
