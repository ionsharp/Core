using System;
using System.Diagnostics;

namespace Imagin.Core.Time
{
    public abstract class BaseTimer
    {
        readonly Stopwatch stopwatch;

        public event EventHandler<EventArgs> Started;

        public event EventHandler<EventArgs> Stopped;

        public event TickEventHandler Tick;

        public virtual TimeSpan Interval { get; set; }

        protected BaseTimer()
        {
            stopwatch = new Stopwatch();
        }

        protected virtual void OnStarted()
        {
            stopwatch.Start();
            Started?.Invoke(this, new EventArgs());
        }

        protected virtual void OnStopped()
        {
            stopwatch.Stop();
            stopwatch.Reset();
            Stopped?.Invoke(this, new EventArgs());
        }

        protected virtual void OnTick()
            => Tick?.Invoke(this, new TickEventArgs(stopwatch.Elapsed));
    }
}