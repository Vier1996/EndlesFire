using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Library.CustomSliders
{
    public class OffsetSlider : MonoBehaviour
    {
        [BoxGroup("Params"), SerializeField] private Image _fillingImage;
        [BoxGroup("Params"), SerializeField] private float _targetWidth;

        private RectTransform _fillingTransform;
        private float _height;
        private float _halfWidth;

        private void Awake()
        {
            _fillingTransform = _fillingImage.transform as RectTransform;
            _height = _fillingTransform.sizeDelta.y;
            _halfWidth = _targetWidth * 0.5f;
        }

        public void SetNormalizedProgress(float progress)
        {
            float nextWidth = _targetWidth * progress;
            float nextPositionX = -_halfWidth + _halfWidth * progress;

            _fillingTransform.sizeDelta = new Vector2(nextWidth, _height);
            _fillingTransform.localPosition = new Vector3(nextPositionX, 0, 0);
        }
    }
}
