using System;

namespace Imagin.Common.Timers
{
    public class Timer : BaseTimer
    {
        readonly System.Timers.Timer timer;

        public override TimeSpan Interval
        {
            get => TimeSpan.FromMilliseconds(timer.Interval);
            set => timer.Interval = value.TotalMilliseconds;
        }

        public Timer() : base()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += OnTick;
        }

        void OnTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            OnTick();
        }

        public void Start()
        {
            timer.Start();
            OnStarted();
        }

        public void Stop()
        {
            timer.Stop();
            OnStopped();
        }
    }
}
