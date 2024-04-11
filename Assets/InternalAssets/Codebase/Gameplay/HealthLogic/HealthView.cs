using System;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.Extension.Rx;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Gameplay.HealthLogic
{
    public class HealthView : MonoBehaviour
    {
        [BoxGroup("CanvasGroup"), SerializeField] private CanvasGroup _selfGroup;
        
        [BoxGroup("Hit"), SerializeField] private Material _hitMaterial;
        [BoxGroup("Hit"), SerializeField] private float _hitTime = 0.075f;
        
        [BoxGroup("Health"), SerializeField] private SpriteRenderer _healthIcon;
        [BoxGroup("Health"), SerializeField] private Transform _healthClosingIconTransform;
        [BoxGroup("Health"), SerializeField] private float _defaultPositionX;
        [BoxGroup("Health"), SerializeField] private float _healthClosingClamp;
        
        private Color _baseHealthColor;
        private bool _inRemovingStatus = false;
        
        private IDisposable _changeHealthMaterialDisposable;

        public void Initialize(float healthRatio)
        {
            _inRemovingStatus = false;
            _baseHealthColor = _healthIcon.color;

            InitializeViews(healthRatio);
        }
        
        private void OnDisable()
        {
            _changeHealthMaterialDisposable?.Dispose();
        }

        public void ChangeColor(Color color) => _baseHealthColor = _healthIcon.color = color;

        private void InitializeViews(float healthRatio)
        {
            float healthPercent = 1f - healthRatio;

            //_healthIcon.fillAmount = healthPercent;
            _healthClosingIconTransform.DOLocalMoveX(_defaultPositionX + _healthClosingClamp * healthPercent, 0);
        }

        public void ChangeHealthProgress(float ratio)
        {
            if(_inRemovingStatus) 
                return;
            
            float percent = 1f - ratio;
            
            _healthIcon.KillTween();
            _healthClosingIconTransform.KillTween();
            
            //_healthIcon.DOFillAmount(percent, _hitTime);
            _healthClosingIconTransform.DOLocalMoveX(_defaultPositionX + _healthClosingClamp * percent, _hitTime);
        }

        public void PlayHealthHitAnimation()
        {
            _healthIcon.color = Color.white;
            _healthIcon.material = _hitMaterial;

            _changeHealthMaterialDisposable?.Dispose();
            _changeHealthMaterialDisposable = RX.Delay(_hitTime, ReturnMaterial);
        }
        
        public void Display() => _selfGroup.alpha = 1;
        
        public void Hide() => _selfGroup.alpha = 0;

        public void Remove()
        {
            _inRemovingStatus = true;
            
            _changeHealthMaterialDisposable?.Dispose();

            _healthIcon.KillTween();
            _healthClosingIconTransform.KillTween();

            _healthIcon.color = _baseHealthColor;
        }

        private void ReturnMaterial()
        {
            _healthIcon.color = _baseHealthColor;
            _healthIcon.material = default;
        }
    }
}