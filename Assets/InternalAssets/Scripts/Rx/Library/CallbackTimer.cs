using System;
using UniRx;
using UnityEngine;

namespace Rx.Library
{
    public static class CallbackTimer
    {
        public static void SetCallbackDelay(this GameObject caller, float delayTime, Action onDelayComplete) =>
            Observable
                .Timer(TimeSpan.FromSeconds(delayTime))
                .AsUnitObservable()
                .Subscribe(_ => onDelayComplete?.Invoke())
                .AddTo(caller);
        
        public static IDisposable SetCallbackDelayWithManualDisposable(float delayTime, Action onDelayComplete) =>
            Observable
                .Timer(TimeSpan.FromSeconds(delayTime))
                .AsUnitObservable()
                .Subscribe(_ => onDelayComplete?.Invoke());
    }
}