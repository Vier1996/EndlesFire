using System;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.Extension.Rx;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HealthLogic
{
    public class HealthSlider : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sliderRenderer;
        [SerializeField] private float _targetOffsetX;
        [SerializeField] private float _animationDuration;
        [SerializeField] private float _animationDelay;

        [SerializeField] private bool _withHitEffect = true;
        [SerializeField, ShowIf(nameof(_withHitEffect))] private Material _hitMaterial;
        [SerializeField, ShowIf(nameof(_withHitEffect))] private float _hitTime = 0.075f;

        private IDisposable _changeHealthMaterialDisposable;
        private IDisposable _updateSizeDisposable;
        private IDisposable _updateSliderDelay;
        private Transform _sliderTransform;
        private Material _defaultMaterial;
        
        private Vector2 _baseScale;
        private Vector3 _baseLocalPosition;
        private Color _baseHealthColor;

        private void Awake()
        {
            _sliderTransform = _sliderRenderer.transform;
            _baseHealthColor = _sliderRenderer.color;
            _baseScale = _sliderRenderer.size;
            _baseLocalPosition = _sliderTransform.localPosition;
            _defaultMaterial = _sliderRenderer.material;
        }

        private void OnDestroy() => _changeHealthMaterialDisposable?.Dispose();

        public HealthSlider UpdateRatio(float ratio)
        {
            _updateSliderDelay = RX.Delay(_animationDelay, () => UpdateValues(ratio));
            
            return this;
        }
        
        public HealthSlider EnableHitAnimation()
        {
            if (_withHitEffect == false) return this;
            
            _sliderRenderer.color = Color.white;
            _sliderRenderer.material = _hitMaterial;

            _changeHealthMaterialDisposable?.Dispose();
            _changeHealthMaterialDisposable = RX.Delay(_hitTime, DisableHitAnimation);

            return this;
        }

        private void UpdateValues(float ratio)
        {
            float positionOffset = Mathf.Lerp(_targetOffsetX, _baseLocalPosition.x, ratio);
            float newSize = Mathf.Lerp(0, _baseScale.x, ratio);

            _sliderTransform.KillTween();
            _sliderTransform.DOLocalMoveX(positionOffset, _animationDuration).SetEase(Ease.Linear);

            _updateSizeDisposable?.Dispose();
            _updateSizeDisposable = RX
                .DoValue(_sliderRenderer.size.x, newSize, _animationDuration)
                .Subscribe(value => _sliderRenderer.size = new Vector2(value, _baseScale.y));
        }
        
        private void DisableHitAnimation()
        {
            _sliderRenderer.color = _baseHealthColor;
            _sliderRenderer.material = _defaultMaterial;
        }
    }
}
