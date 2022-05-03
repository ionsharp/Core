using System;

namespace Imagin.Common.Timers
{
    public class DispatcherTimer : BaseTimer
    {
        readonly System.Windows.Threading.DispatcherTimer timer;

        public override TimeSpan Interval
        {
            get => timer.Interval;
            set => timer.Interval = value;
        }

        public DispatcherTimer() : base()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += OnTick;
        }

        void OnTick(object sender, EventArgs e)
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