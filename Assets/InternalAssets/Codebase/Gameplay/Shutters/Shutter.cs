using System;
using System.Collections.Generic;
using Codebase.Library.Extension.Dotween;
using Codebase.Library.Extension.MonoBehavior;
using Codebase.Library.Extension.Rx;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace InternalAssets.Codebase.Gameplay.Shutters
{
    public class Shutter : MonoBehaviour
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

        private void Awake()
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
        }

        [Button]
        public async void DisplayShutter()
        {
            await _background.DOFade(1f, 0.5f).AsyncWaitForCompletion();
            
            ShutterOpen?.Invoke();
            
            DisplayItems(StartAnimation);
        }

        [Button]
        public void HideShutter()
        {
            StopAnimation();
            
            HideItems(HideBackground);
        }

        private void HideBackground() =>
            _background
                .DOFade(0f, 0.5f)
                .OnComplete(() => ShutterClosed?.Invoke());

        private void StartAnimation()
        {
            _animateDisposable?.Dispose();
            _animateDisposable = RX.LoopedTimer(0.25f, _delayBetweenCommonJumping, AnimateElements);
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

            Shuffle(_possibleIcons);
            
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
        
        private void Shuffle<T>(IList<T> list)
        {
            Random random = new Random();
            
            int n = list.Count;
            while (n > 1)
            {
                n--;
                
                int k = random.Next(n + 1);
                
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
