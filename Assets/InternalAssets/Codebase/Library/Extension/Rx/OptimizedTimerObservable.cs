using System;
using UniRx;
using UniRx.Operators;

namespace Codebase.Library.Extension.Rx
{
    public class OptimizedTimerObservable : OperatorObservableBase<long>
    {
        public event Action<OptimizedTimerObservable> Disposed;
        
        private DateTimeOffset? _dueTimeA;
        private TimeSpan? _dueTimeB;
        private TimeSpan? _period;
        private IScheduler _scheduler;
        private SingleAssignmentDisposable _disposable = new();

        public OptimizedTimerObservable() : base(false) { }
        
        public OptimizedTimerObservable(DateTimeOffset dueTime, TimeSpan? period, IScheduler scheduler)
            : base(scheduler == Scheduler.CurrentThread)
        {
            _dueTimeA = dueTime;
            _period = period;
            _scheduler = scheduler;
        }

        public OptimizedTimerObservable(TimeSpan dueTime, TimeSpan? period, IScheduler scheduler)
            : base(scheduler == Scheduler.CurrentThread)
        {
            _dueTimeB = dueTime;
            _period = period;
            _scheduler = scheduler;
        }

        public OptimizedTimerObservable Dispose()
        {
            if(_disposable.IsDisposed == false)
                _disposable.Dispose();
            
            return this;
        }
        
        public OptimizedTimerObservable Initialize(TimeSpan dueTime, TimeSpan? period, IScheduler scheduler)
        {
            _dueTimeB = dueTime;
            _period = period;
            _scheduler = scheduler;

            return this;
        }

        public IDisposable BindDisposable(SingleAssignmentDisposable disposable)
        {
            _disposable = disposable;
            _disposable.Disposed += NotifyDispose;

            return _disposable;
        }

        protected override IDisposable SubscribeCore(IObserver<long> observer, IDisposable cancel)
        {
            var timerObserver = new Timer(observer, cancel);

            var dueTime = (_dueTimeA != null)
                ? _dueTimeA.Value - _scheduler.Now
                : _dueTimeB.Value;

            // one-shot
            if (_period == null)
            {
                return _scheduler.Schedule(Scheduler.Normalize(dueTime), () =>
                {
                    timerObserver.OnNext();
                    timerObserver.OnCompleted();
                });
            }
            else
            {
                var periodicScheduler = _scheduler as ISchedulerPeriodic;
                if (periodicScheduler != null)
                {
                    if (dueTime == _period.Value)
                    {
                        // same(Observable.Interval), run periodic
                        return periodicScheduler.SchedulePeriodic(Scheduler.Normalize(dueTime), timerObserver.OnNext);
                    }
                    else
                    {
                        // Schedule Once + Scheudle Periodic
                        var disposable = new SerialDisposable();

                        disposable.Disposable = _scheduler.Schedule(Scheduler.Normalize(dueTime), () =>
                        {
                            timerObserver.OnNext(); // run first

                            var timeP = Scheduler.Normalize(_period.Value);
                            disposable.Disposable = periodicScheduler.SchedulePeriodic(timeP, timerObserver.OnNext); // run periodic
                        });

                        return disposable;
                    }
                }
                else
                {
                    var timeP = Scheduler.Normalize(_period.Value);

                    return _scheduler.Schedule(Scheduler.Normalize(dueTime), self =>
                    {
                        timerObserver.OnNext();
                        self(timeP);
                    });
                }
            }
        }

        private void NotifyDispose() => Disposed?.Invoke(this);

        private class Timer : OperatorObserverBase<long, long>
        {
            long index = 0;

            public Timer(IObserver<long> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public void OnNext()
            {
                try
                {
                    base.observer.OnNext(index++);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public override void OnNext(long value)
            {
                // no use.
            }

            public override void OnError(Exception error)
            {
                try { observer.OnError(error); }
                finally { Dispose(); }
            }

            public override void OnCompleted()
            {
                try { observer.OnCompleted(); }
                finally { Dispose(); }
            }
        }
    }
}