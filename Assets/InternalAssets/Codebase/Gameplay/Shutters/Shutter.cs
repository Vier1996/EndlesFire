using System;
using System.Collections.Generic;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.Extension.MonoBehavior;
using Codebase.Library.Extension.Rx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Gameplay.Shutters
{
    public class Shutter : MonoBehaviour, IShutter
    {
        public event Action ShutterOpen;
        public event Action ShutterClosed;
        
        [SerializeField] private Image _background;
        [SerializeField] private List<Image> _icons = new();
        [SerializeField] private List<Sprite> _possibleIcons = new();
        
        [BoxGroup("Animation params"), SerializeField] private float _betweenElementsDelay = 0.1f;
        [BoxGroup("Animation params"), SerializeField] private float _delayBetweenCommonJumping = 1f;
        [BoxGroup("Animation params"), SerializeField] private float _betweenChangingSpriteDelay = 0.1f;
        [BoxGroup("Animation params"), SerializeField] private float _changeSpritesDuration = 0.2f;
        [BoxGroup("Animation params"), SerializeField] private float _delayAfterSpriteSwapping = 0.2f;

        private IDisposable _animateDisposable;
        private IDisposable _animationQueueElementsDisposable;
        private IDisposable _changeSpriteDisposable;

        private bool _isActivated = false;

        private void Awake()
        {
            InstantHide();
            
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        [Button]
        public async UniTask DisplayShutter()
        {
            if(_isActivated) return;

            _isActivated = true;
            
            gameObject.SetActive(true);
            
            _background.KillTween();
            
            await _background.DOFade(1f, 0.5f).AsyncWaitForCompletion();
            
            DisplayItems(StartAnimation);
        }

        [Button]
        public void HideShutter()
        {
            if(_isActivated == false) return;

            _isActivated = false;
            
            StopAnimation();
            
            HideItems(HideBackground);
        }

        private void InstantHide()
        {
            Color currentBackgroundColor = _background.color;
            
            _background.color = new Color(
                currentBackgroundColor.r,
                currentBackgroundColor.g,
                currentBackgroundColor.b,
                0
            );
            
            foreach (Image targetImage in _icons)
                targetImage.transform.localScale = Vector3.zero;
            
            gameObject.SetActive(false);
        }

        private void HideBackground() =>
            _background
                .DOFade(0f, 0.5f)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    
                    ShutterClosed?.Invoke();
                });

        private void StartAnimation()
        {
            _animateDisposable?.Dispose();
            _animateDisposable = RX.LoopedTimer(0.25f, _delayBetweenCommonJumping, AnimateElements);
            
            ShutterOpen?.Invoke();
        }
        
        private void StopAnimation()
        {
            _animationQueueElementsDisposable?.Dispose();
            _animateDisposable?.Dispose();
            _changeSpriteDisposable?.Dispose();
        }

        private void DisplayItems(Action onDisplayed)
        {
            _animateDisposable = RX.CountedTimer(0f, 0.1f, _icons.Count,
                index =>
                {
                    _icons[index].transform.DisplayBubbled(0.9f, 0.1f, defaultScale: 0.8f);
                }, totalCompleteCallback: onDisplayed);
        }
        
        private void HideItems(Action onDisplayed)
        {
            _animationQueueElementsDisposable?.Dispose();
            _animateDisposable?.Dispose();
            _changeSpriteDisposable?.Dispose();
            
            _animateDisposable = RX.CountedTimer(0f, 0.1f, _icons.Count,
                index =>
                {
                    Transform imageTransform = _icons[index].transform;
                    Vector3 position = imageTransform.localPosition;

                    imageTransform.KillTween();
                    imageTransform.DisplayBubbled(0.9f, 0.1f, defaultScale: 0f, onComplete: () =>
                    {
                        imageTransform.localPosition = new Vector3(position.x, 0, 0);
                        imageTransform.localEulerAngles = Vector3.zero; 
                    });
                }, 
                totalCompleteCallback: onDisplayed);
        }

        private void AnimateElements()
        {
            _changeSpriteDisposable?.Dispose();
            _animationQueueElementsDisposable?.Dispose();

            _possibleIcons.Shuffle();
            
            _changeSpriteDisposable = RX.CountedTimer(0f, _betweenChangingSpriteDelay, _icons.Count, 
                index => ChangeSprite(_icons[index], index),
                totalCompleteCallback: JumpElements);
        }

        private void JumpElements()
        {
            _animationQueueElementsDisposable = RX.CountedTimer(_delayAfterSpriteSwapping, _betweenElementsDelay, _icons.Count, 
                index => SetJumpingAnimationToImage(_icons[index].transform));
        }

        private void ChangeSprite(Image targetImage, int index)
        {
            targetImage
                .DOFade(0f, _changeSpritesDuration * 0.5f)
                .OnComplete(() =>
                {
                    targetImage.sprite = _possibleIcons[index];
                    targetImage.DOFade(1f, _changeSpritesDuration * 0.5f);
                });
        }

        private void SetJumpingAnimationToImage(Transform imageTransform)
        {
            imageTransform.KillTween();
            
            imageTransform.DOScale(new Vector3(0.8f, 0.6f, 0.8f), 0.25f);
            imageTransform
                .DOLocalMoveY(-10f, 0.25f)
                .OnComplete(() =>
                {
                    imageTransform.DOLocalRotate(new Vector3(0, 0, -360), 0.75f, RotateMode.LocalAxisAdd);
                    imageTransform.DOScale(new Vector3(0.8f, 0.9f, 0.8f), 0.75f);
                    imageTransform
                        .DOLocalMoveY(50f, 0.5f)
                        .OnComplete(() =>
                        {
                            imageTransform.DOScale(new Vector3(0.8f, 0.6f, 0.8f), 0.25f);
                            imageTransform
                                .DOLocalMoveY(-10f, 0.25f)
                                .OnComplete(() =>
                                {
                                    imageTransform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.25f);
                                    imageTransform.DOLocalMoveY(0f, 0.25f);
                                });
                        });
                });
        }
        
        private void OnSceneChanged(Scene s1, Scene s2)
        {
            HideShutter();
        }
    }
}
