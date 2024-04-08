using System;
using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons
{
    public class WeaponSpark : MonoBehaviour, IDisposable
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private Color _emptyColor;

        private int _spriteIndex = 0;

        public void Dispose() => _renderer.KillTween();
        
        public void ShowSpark()
        {
            Break();

            _renderer.sprite = _sprites[_spriteIndex];
            _spriteIndex = GetNextIndex();

            ShowAnimation();
        }

        private void ShowAnimation() =>
            _renderer.DOFade(1f, 0.1f)
                .OnComplete(() => _renderer.DOFade(0f, 0.1f));

        private void Break()
        {
            _renderer.KillTween();
            _renderer.color = _emptyColor;
        }
    
        private int GetNextIndex()
        {
            if (_spriteIndex >= _sprites.Length - 1) _spriteIndex = 0;
            else _spriteIndex++;
            
            return _spriteIndex;
        }
    }
}