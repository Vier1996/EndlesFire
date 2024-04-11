using Codebase.Library.Extension.Dotween;
using DG.Tweening;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Targets
{
    public class TargetView : MonoBehaviour
    {
        [SerializeField] private Color _targetColor;
        [SerializeField] private SpriteRenderer _targetableSprite;

        private void Start()
        {
            _targetableSprite.gameObject.SetActive(false);
            _targetableSprite.color = _targetColor;
        }

        public void EnableView()
        {
            _targetableSprite.KillTween();
            _targetableSprite
                .DOFade(1f, 0.5f)
                .OnStart(() => _targetableSprite.gameObject.SetActive(true));
        }

        public void DisableView()
        {
            _targetableSprite.KillTween();
            _targetableSprite
                .DOFade(0f, 0.5f)
                .OnStart(() => _targetableSprite.gameObject.SetActive(false));
        }
    }
}
