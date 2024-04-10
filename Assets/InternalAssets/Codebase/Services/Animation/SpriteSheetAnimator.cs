using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.SAD;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Services.Animation
{
    public class SpriteSheetAnimator : MonoBehaviour, IDerivedEntityComponent
    {
        public event Action AnimationFinished;
        
        [BoxGroup("Default"), SerializeField] private bool _startFromDefaultAnimation = false;
        [BoxGroup("Renderer"), SerializeField] private SpriteRenderer _targetRenderer;
        [BoxGroup("Animation"), SerializeField] private List<SpriteSheetAnimationSetup> _animationSetups;

        private IDisposable _updateAnimationDisposable;
        private Sprite _defaultSprite;
        private SpriteSheetAnimationSetup _currentAnimationSetup = null;

        private float _updateNextSpriteTimer = 0f;
        private bool _enabled = false;
        private bool _once = false;
        private int _spriteIndex = 0;
        
        public void Bootstrapp(Entity entity)
        {
            _updateAnimationDisposable = Observable.EveryFixedUpdate().Subscribe(_ => UpdateAnimation());
            
            if(_startFromDefaultAnimation == false)
                return;
            
            SpriteSheetAnimationSetup setup = _animationSetups.First();
            
            SetAnimation(setup.AnimationType);
        }
        
        public void Dispose()
        {
            _updateAnimationDisposable?.Dispose();
            _enabled = false;
        }

        public void SetAnimation(CommonAnimationType animationType)
        {
            if (_currentAnimationSetup != null && _currentAnimationSetup.AnimationType == animationType)
                return;

            _currentAnimationSetup = _animationSetups.FirstOrDefault(stp => stp.AnimationType == animationType);

            if (_currentAnimationSetup == default)
                throw new ArgumentException($"Trying to call non present animation with type:{animationType}");

            _enabled = true;
        }
        
        public void PlayAnimationOnce(CommonAnimationType animationType)
        {
            _currentAnimationSetup = _animationSetups.FirstOrDefault(stp => stp.AnimationType == animationType);

            if (_currentAnimationSetup == default)
                throw new ArgumentException($"Trying to call non present animation with type:{animationType}");

            _once = true;
            _enabled = true;
        }

        public float AnimationLength(CommonAnimationType animationType)
        {
            SpriteSheetAnimationSetup setup = _animationSetups.FirstOrDefault(stp => stp.AnimationType == animationType);
            
            if (setup == default) 
                return 0;
            
            return setup.AnimationSprites.Length * setup.UpdateTimer;
        }

        public void UpdateSize(float sizeMultiplier, float time = 0)
        {
            _targetRenderer.transform.KillTween();
            _targetRenderer.transform.DOScale(sizeMultiplier, time);
        }

        private void UpdateAnimation()
        {
            if (!_enabled)
                return;

            if (_updateNextSpriteTimer > 0)
            {
                _updateNextSpriteTimer -= Time.fixedDeltaTime;
                return;
            }

            ChangeSprite();
        }

        private void ChangeSprite()
        {
            if (_currentAnimationSetup == null)
            {
                AnimationFinished?.Invoke();
                _enabled = false;
                return;
              
            }
            
            _updateNextSpriteTimer = _currentAnimationSetup.UpdateTimer;
            int nextFrame = GetNextIndex();
            
            _targetRenderer.sprite = _currentAnimationSetup.AnimationSprites[nextFrame];

            if (nextFrame == _currentAnimationSetup.AnimationSprites.Length - 1)
            {
                AnimationFinished?.Invoke();

                if (_once)
                {
                    _enabled = false;
                    _targetRenderer.sprite = _defaultSprite;
                }
            }
        }

        private int GetNextIndex()
        {
            if (_spriteIndex >= _currentAnimationSetup.AnimationSprites.Length - 1)
                _spriteIndex = 0;
            else
                _spriteIndex++;
            
            return _spriteIndex;
        }
    }
    
    [Serializable]
    public class SpriteSheetAnimationSetup
    {
        [field: SerializeField] public CommonAnimationType AnimationType { get; private set; } = CommonAnimationType.none;
        [field: SerializeField] public float UpdateTimer { get; private set; } = 0.1f;
        [field: SerializeField] public Sprite[] AnimationSprites { get; private set; } = {};
    }
}
