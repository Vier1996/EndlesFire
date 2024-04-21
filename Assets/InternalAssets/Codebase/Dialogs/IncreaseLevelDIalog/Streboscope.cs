using System;
using Codebase.Library.Extension.Rx;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Codebase.Dialogs.IncreaseLevelDialog
{
    public class Streboscope : MonoBehaviour
    {
        [SerializeField] private Image _blueLight;
        [SerializeField] private Image _redLight;
        
        [SerializeField] private float _lightDuration;
        [SerializeField] private float _lightDelay;
        [SerializeField] private float _initialDelay;

        private IDisposable _workingDisposable;
        
        [Button]
        private void Start()
        {
            _workingDisposable = RX.LoopedTimer(_initialDelay, _lightDelay, SwitchOnLights);
        }

        [Button]
        private void OnDestroy()
        {
            _workingDisposable?.Dispose();
        }

        private void SwitchOnLights()
        {
            LightBlue();
            LightRed();
        }

        private void LightBlue()
        {
            _blueLight.DOFade(1f, _lightDuration * 0.5f).SetLoops(2, LoopType.Yoyo);
        }

        private void LightRed()
        {
            _redLight.DOFade(1f, _lightDuration * 0.25f).SetLoops(4, LoopType.Yoyo);
        }
    }
}
