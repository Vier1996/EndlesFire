using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Materials.Shine
{
    public class ShineEffect : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _duration;
        [SerializeField] private float _delay;
        [SerializeField] private float _startDelay;

        private Coroutine _shineCoroutine;
        private WaitForSeconds _delaySeconds;

        private Material _material;
        private static readonly int ShineLocation = Shader.PropertyToID("_ShineLocation");

        private void Awake()
        {
            _material = _image.material;
            _delaySeconds = new WaitForSeconds(_duration + _delay);
        }

        private void Start()
        {
            BreakShine();
            _shineCoroutine = StartCoroutine(Shine());
        }

        private void BreakShine() => _material.SetFloat(ShineLocation, 1f);

        private IEnumerator Shine()
        {
            yield return new WaitForSeconds(_startDelay);
            
            while (true)
            { 
                _material.DOFloat(0f, ShineLocation, _duration).SetEase(Ease.Linear).OnComplete(BreakShine);

                yield return _delaySeconds;
            }
        }

        private void OnDestroy()
        {
            if(_shineCoroutine != null)
                StopCoroutine(_shineCoroutine);
            
            _material.DOPause();
            _material.DOKill();
        }
    }
}
