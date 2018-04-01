using System;
using System.Timers;
using System.Xml.Serialization;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PeriodicalObject : DisposableObject
    {
        /// <summary>
        /// The default interval to use.
        /// </summary>
        public readonly static TimeSpan DefaultInterval = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Occurs when the timer elapses.
        /// </summary>
        public event ElapsedEventHandler Elapsed;

        /// <summary>
        /// 
        /// </summary>
        Timer _timer;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool Enabled
        {
            get => _timer.Enabled;
            set => _timer.Enabled = value;
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public TimeSpan Interval
        {
            get => TimeSpan.FromMilliseconds(_timer.Interval);
            set => _timer.Interval = value.TotalMilliseconds;
        }

        /// <summary>
        /// Initializes an instance of <see cref="PeriodicalObject"/>.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public PeriodicalObject(TimeSpan? interval = null, bool enabled = false) : base()
        {
            _timer = new Timer();
            _timer.Elapsed += OnElapsed;

            Interval = interval ?? DefaultInterval;
            Enabled = enabled;
        }

        void OnElapsed(object sender, ElapsedEventArgs e)
            => OnElapsed(e);

        /// <summary>
        /// Occurs when the timer elapses.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnElapsed(ElapsedEventArgs e) => Elapsed?.Invoke(this, e);

        /// <summary>
        /// 
        /// </summary>
        protected override void OnUnmanagedDisposed()
        {
            base.OnUnmanagedDisposed();
            _timer.Enabled = false;
            _timer.Elapsed -= OnElapsed;
            _timer.Dispose();
        }
    }
}
