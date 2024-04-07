using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Library.Resizers
{
    public class CanvasResizer : MonoBehaviour
    {
        public bool Resized { get; private set; } = false;
        
        private const float BaseCanvasHeight = 1280f;
        private const float BaseCanvasWidth = 720f;
        private const float BaseScreenRatio = 16 / 9f;

        private void Awake() => SetupCanvas();

        private void SetupCanvas()
        {
            CanvasScaler scaler = GetComponent<CanvasScaler>();

            float expanding = Mathf.Max(1f, Screen.height / (float)Screen.width / BaseScreenRatio);
            
            scaler.referenceResolution = new Vector2(
                BaseCanvasWidth,
                BaseCanvasHeight);

            transform.DOScale(0.01f, 0).OnComplete(() => Resized = true);
        }
    }
}