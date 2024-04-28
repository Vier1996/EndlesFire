using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HUDs
{
    public class HudElement : MonoBehaviour
    {
        [BoxGroup("Enable params"), SerializeField] private float _enableDuration = 0f;
        [BoxGroup("Enable params"), SerializeField] private float _enableDelay = 0f;
        
        [BoxGroup("Disable params"), SerializeField] private float _disableOffsetValue = 0f;
        [BoxGroup("Disable params"), SerializeField] private float _disableDuration = 0f;
        [BoxGroup("Disable params"), SerializeField] private float _disableDelay = 0f;
        
        private RectTransform _selfRectTransform;
        private Vector2 _defaultPosition;

        public void Initialize()
        {
            _selfRectTransform = transform as RectTransform;
            _defaultPosition = _selfRectTransform.anchoredPosition;
        }
        
        public float Enable(bool instant = false)
        {
            if (instant)
            {
                _selfRectTransform.anchoredPosition = _defaultPosition;
                return 0f;
            }
            
            _selfRectTransform
                .DOAnchorPosY(_defaultPosition.y, _enableDuration)
                .SetDelay(_enableDelay);
            
            return _enableDuration + _enableDelay;
        }
        
        public float Disable(bool instant = false)
        {
            if (instant)
            {
                _selfRectTransform.anchoredPosition = new Vector2(_defaultPosition.x, _defaultPosition.y + _disableOffsetValue);
                return 0f;
            }
            
            _selfRectTransform
                .DOAnchorPosY(_defaultPosition.y + _disableOffsetValue, _disableDuration)
                .SetDelay(_disableDelay);
            
            return _disableDuration + _disableDelay;
        }
    }
}