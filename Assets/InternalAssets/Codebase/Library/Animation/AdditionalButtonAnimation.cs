using System;
using Codebase.Library.Extension;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Codebase.Library.Animation
{
    public class AdditionalButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Button Button => _selfButton;
        public RectTransform RectTransform => _selfRectTransform;
        
        [OnValueChanged(nameof(RecalculateValues))]
        [SerializeField] private ButtonAnimationScalingOptions _options;

        private bool _initialized = false;
        
        private float _scalingAnimationTime;
        private float _bounceAnimationTime;
        private float _scaleFactor;
        private float _bounceScaleFactor;
        
        private Vector3 _targetScale;
        private Vector3 _defaultScale;
        private Vector3 _bounceScaleVector;
        private Vector3 _bounceUnscaleVector;
        
        private Button _selfButton;
        private RectTransform _selfRectTransform;

        private void Awake() => Initialize();

        private void Start() => _selfButton.transition = Selectable.Transition.None;

        public void Initialize()
        {
            if(_initialized) return;

            _initialized = true;
            
            _selfButton = gameObject.GetComponent<Button>();
            _selfRectTransform = gameObject.GetComponent<RectTransform>();
            _defaultScale = _selfRectTransform.localScale;

            RecalculateValues();
        }
        
        public void RefreshValues() => RecalculateValues();
        
        public void OnPointerDown(PointerEventData eventData) => ScaleButton(_targetScale, _bounceScaleVector);

        public void OnPointerUp(PointerEventData eventData) => ScaleButton(_defaultScale, _bounceUnscaleVector);

        public void UpdateValues(Vector3 defaultScale)
        {
            _defaultScale = defaultScale;
            
            RecalculateValues();
        }
        
        private void RecalculateValues()
        {
            _scalingAnimationTime = _options.AnimationSpeed * (_options.AddBounceEffect ? 1f - _options.BounceBalance : 1f);
            _scaleFactor = 1f + _options.ScalingIntensity * (_options.IsExpanding ? 1f : -1f);
            _targetScale = _defaultScale * _scaleFactor;

            if (_options.AddBounceEffect)
            {
                _bounceAnimationTime = _options.AnimationSpeed * (_options.AddBounceEffect ? _options.BounceBalance : 0f) * .5f;
                _bounceScaleFactor = 1f + (1f - _scaleFactor) * .5f;
                _bounceScaleVector = _targetScale * _bounceScaleFactor;
                _bounceUnscaleVector = _defaultScale * (1f + (1f - _bounceScaleFactor));
            }
        }

        private void ScaleButton(Vector3 targetVector, Vector3 bounceVector)
        {
            KillTween();
            
            _selfRectTransform
                .DOScale(targetVector, _scalingAnimationTime)
                .SetEase(_options.AnimationEase).OnComplete(() =>
                {
                    if (_options.AddBounceEffect)
                    {
                        _selfRectTransform
                            .DOScale(bounceVector, _bounceAnimationTime)
                            .OnComplete(() => _selfRectTransform.DOScale(targetVector, _bounceAnimationTime))
                            .SetUpdate(true);
                    }
                })
                .SetUpdate(true);
        }

        private void KillTween() => _selfRectTransform.KillTween();
        

        [Serializable]
        public class ButtonAnimationScalingOptions
        {
            [Range(0f, 1f)] public float AnimationSpeed = 0.2f;
            [Range(0f, 1f)] public float ScalingIntensity = 0.1f;
            public Ease AnimationEase = Ease.OutBack;
            public bool AddBounceEffect = false;
            [ShowIf("@AddBounceEffect == true")] public float BounceBalance = 0.2f;
            public bool IsExpanding = false;
        }
        
#if UNITY_EDITOR
        
        [Button] private void ImitatePress()
        {
            ScaleButton(_targetScale, _bounceScaleVector);
        }
        
        [Button] private void ImitateRelease()
        {
            ScaleButton(_defaultScale, _bounceUnscaleVector);
        }
        
#endif
    }
}
