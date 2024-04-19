using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Services.Animation
{
    public class SpriteSheetAnimator : MonoBehaviour
    {
        public event Action AnimationFinished;
        
        [BoxGroup("Default"), SerializeField] private bool _startFromDefaultAnimation = false;
        [BoxGroup("Renderer"), SerializeField] private SpriteRenderer _targetRenderer;
        [BoxGroup("Animation"), SerializeField] private List<SpriteSheetAnimationSetup> _animationSetups = new();

        private IDisposable _updateAnimationDisposable;
        private Sprite _defaultSprite;
        private SpriteSheetAnimationSetup _currentAnimationSetup = null;

        private CommonAnimationType _currentAnimation = CommonAnimationType.none;
        private AwaitAnimationArgs _awaitAnimationArgs;
        
        private float _updateNextSpriteTimer = 0f;
        private bool _enabled = false;
        private bool _once = false;
        private bool _await = false;
        private int _spriteIndex = 0;

        private void ResetComponent()
        {
            _currentAnimation = CommonAnimationType.none;
            _spriteIndex = 0;
            
            _updateAnimationDisposable?.Dispose();
        }

        public SpriteSheetAnimator Bootstrapp()
        {
            ResetComponent();
            
            _updateAnimationDisposable = Observable.EveryFixedUpdate().Subscribe(_ => UpdateAnimation());
            
            if(_startFromDefaultAnimation == false)
                return this;
            
            SetAnimation(_animationSetups.First().AnimationType);

            return this;
        }

        public void Dispose()
        {
            _updateAnimationDisposable?.Dispose();
            _enabled = false;
            
            AnimationFinished -= OnAnimationFinished;
        }

        [Button]
        public void SetAnimation(CommonAnimationType animationType, bool once = false, bool force = false, bool await = false)
        {
            if (animationType == CommonAnimationType.none || (force == false && _currentAnimation == animationType))
                return;

            if (_await)
            {
                _awaitAnimationArgs = new AwaitAnimationArgs()
                {
                    AnimationType = animationType,
                    Once = once,
                    Force = force,
                    Await = @await
                };
                
                return;
            }

            _await = await;
            _currentAnimationSetup = _animationSetups.FirstOrDefault(stp => stp.AnimationType == animationType);
            
            if (_currentAnimationSetup == default)
                throw new ArgumentException($"Trying to call non present animation with type:{animationType}");

            _currentAnimation = _currentAnimationSetup.AnimationType;
            _enabled = true;
            _once = once;
            _spriteIndex = 0;
            
            if (_await) AnimationFinished += OnAnimationFinished;
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
        
        private void OnAnimationFinished()
        {
            AnimationFinished -= OnAnimationFinished;

            if (_await && _awaitAnimationArgs.Equals(default) == false)
            {
                _await = false;
                
                SetAnimation(_awaitAnimationArgs.AnimationType, _awaitAnimationArgs.Once, _awaitAnimationArgs.Force, _awaitAnimationArgs.Await);
            }
        }
    }
    
    [Serializable]
    public class SpriteSheetAnimationSetup
    {
        [field: SerializeField] public CommonAnimationType AnimationType { get; private set; } = CommonAnimationType.none;
        [field: SerializeField] public float UpdateTimer { get; private set; } = 0.1f;
        [field: SerializeField] public Sprite[] AnimationSprites { get; private set; } = {};
    }

    public struct AwaitAnimationArgs
    {
        public CommonAnimationType AnimationType;
        public bool Once;
        public bool Force;
        public bool Await;
    }
}
