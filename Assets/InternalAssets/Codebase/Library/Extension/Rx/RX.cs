using System;
using UniRx;
using UnityEngine;

namespace Codebase.Library.Extension.Rx
{
    public static class RX
    {
        private static readonly ObservableTimersPool _timersPool = new();

        public static IDisposable LoopedTimer(float initialDelay, float interval, Action callback)
        {
            OptimizedTimerObservable timer = _timersPool.SelectAvailable();
            SingleAssignmentDisposable disposableOperation = timer
                .Initialize(TimeSpan.FromSeconds(initialDelay), TimeSpan.FromSeconds(interval), Scheduler.DefaultSchedulers.TimeBasedOperations)
                .Subscribe(_ => callback?.Invoke())
                .AsSingleAssignmentDisposable();
            
            return timer.BindDisposable(disposableOperation);
        }

        public static IDisposable CountedTimer(float initialDelay, float interval, int repeatingCount, Action callback, Action totalCompleteCallback = null)
        {
            OptimizedTimerObservable timer = _timersPool.SelectAvailable();
            SingleAssignmentDisposable disposableOperation = timer
                .Initialize(TimeSpan.FromSeconds(initialDelay), TimeSpan.FromSeconds(interval), Scheduler.DefaultSchedulers.TimeBasedOperations)
                .Take(repeatingCount)
                .Subscribe(_ => callback?.Invoke(), () => totalCompleteCallback?.Invoke())
                .AsSingleAssignmentDisposable();
            
            return timer.BindDisposable(disposableOperation);
        }

        public static IDisposable CountedTimer(float initialDelay, float interval, int repeatingCount, Action<int> callback, Action totalCompleteCallback = null)
        {
            OptimizedTimerObservable timer = _timersPool.SelectAvailable();
            SingleAssignmentDisposable disposableOperation = timer
                .Initialize(TimeSpan.FromSeconds(initialDelay), TimeSpan.FromSeconds(interval), Scheduler.DefaultSchedulers.TimeBasedOperations)
                .Take(repeatingCount)
                .Subscribe(iteration => callback?.Invoke((int)iteration), () => totalCompleteCallback?.Invoke())
                .AsSingleAssignmentDisposable();
            
            return timer.BindDisposable(disposableOperation);
        }

        public static IDisposable Delay(float delay, Action callback, IScheduler scheduler = null)
        {
            OptimizedTimerObservable timer = _timersPool.SelectAvailable();
            SingleAssignmentDisposable disposableOperation = timer
                .Initialize(TimeSpan.FromSeconds(delay), TimeSpan.FromSeconds(0), Scheduler.DefaultSchedulers.TimeBasedOperations)
                .Take(1)
                .Subscribe(_ => callback?.Invoke())
                .AsSingleAssignmentDisposable();
            
            return timer.BindDisposable(disposableOperation);
        }

        public static IObservable<float> DoValue(float startValue, float endValue, float duration, Action onComplete = null) =>
            Observable.Create<float>(observer =>
            {
                float startTime = Time.realtimeSinceStartup;
                float progress = 0.0f;

                void UpdateProgress()
                {
                    float elapsed = Time.realtimeSinceStartup - startTime;
                    progress = Mathf.Clamp01(elapsed / duration);

                    if (progress >= 1.0f)
                    {
                        observer.OnNext(endValue);
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnNext(Mathf.Lerp(startValue, endValue, progress));
                    }
                }

                UpdateProgress();

                return new CompositeDisposable
                {
                    Observable.EveryUpdate().Subscribe(_ => UpdateProgress()),
                    Disposable.Create(() => onComplete?.Invoke())
                };
            });

        public static IObservable<long> DoValue(long startValue, long endValue, float duration, Action onComplete = null) =>
            Observable.Create<long>(observer =>
            {
                float startTime = Time.time;
                float progress = 0.0f;
                float step = endValue - startValue;

                void UpdateProgress()
                {
                    progress = Mathf.Clamp01((Time.time - startTime) / duration);

                    if (progress >= 1.0f)
                    {
                        observer.OnNext(endValue);
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnNext(startValue + (long)(step * progress));
                    }
                }

                UpdateProgress();

                return new CompositeDisposable
                {
                    Observable.EveryUpdate().Subscribe(_ => UpdateProgress()),
                    Disposable.Create(() => onComplete?.Invoke())
                };
            });
        
        public static IDisposable DoTime(float time, Action<float> perTick, Action complete = null)
        {
            float timer = 0;
            return Observable
                .EveryUpdate()
                .TakeWhile(_ => timer < time).Subscribe(_ =>
                {
                    timer += Time.deltaTime;
                    perTick?.Invoke(timer / time);
                }, () =>
                {
                    complete?.Invoke();
                });
        }

        public static Disposable.AnonymousDisposable AsAnonymousDisposable(this IDisposable disposable) => disposable as Disposable.AnonymousDisposable;
        public static SerialDisposable AsSerialDisposable(this IDisposable disposable) => disposable as SerialDisposable;
        public static SingleAssignmentDisposable AsSingleAssignmentDisposable(this IDisposable disposable) => disposable as SingleAssignmentDisposable;
        public static MultipleAssignmentDisposable AsMultipleAssignmentDisposable(this IDisposable disposable) => disposable as MultipleAssignmentDisposable;
        
        public static void CheckListsSize() => _timersPool.CheckListsSize();
    }
}
