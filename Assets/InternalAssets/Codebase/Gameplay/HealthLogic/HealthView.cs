using System.Collections.Generic;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.Extension.MonoBehavior;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HealthLogic
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private List<HealthSlider> _sliders = new();

        private Transform _selfTransform;
        private bool _isDisplayed = true;
        
        public HealthView Initialize(ReactiveProperty<float> reactiveHealthRatio)
        {
            _selfTransform ??= transform;
            
            _sliders.ForEach(sl => sl.UpdateRatio(reactiveHealthRatio.Value));
            
            reactiveHealthRatio.Skip(1).Subscribe(OnRatioChanged);

            return this;
        }

        public HealthView SetActiveStatus(bool status)
        {
            gameObject.SetActive(status);

            return this;
        }

        public HealthView Show(bool instant = false)
        {
            if(_isDisplayed) return this;
            
            _isDisplayed = true;
            
            _selfTransform.KillTween();

            if (instant)
            {
                _selfTransform.localScale = Vector3.one;
                return this;
            }
            
            _selfTransform.DisplayBubbled(1.2f, 0.5f, defaultScale: 1f);
            
            return this;
        }

        public HealthView Hide(bool instant = false)
        {
            if(_isDisplayed == false) return this;
            
            _isDisplayed = false;
            
            _selfTransform.KillTween();
            
            if (instant)
            {
                _selfTransform.localScale = Vector3.zero;
                return this;
            }
            
            _selfTransform.DisplayBubbled(1.1f, 0.5f, defaultScale: 0f);
            
            return this;
        }
        
        private void OnRatioChanged(float ratio) => 
            _sliders
                .ForEach(sl => 
                    sl
                        .EnableHitAnimation()
                        .UpdateRatio(ratio));
    }
}