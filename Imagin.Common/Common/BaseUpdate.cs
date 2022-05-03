using Imagin.Common.Linq;
using System;
using System.Timers;

namespace Imagin.Common
{
    /// <inheritdoc/>
    [Serializable]
    public class BaseUpdate : BaseDisposable
    {
        /// <summary>
        /// The default interval to use.
        /// </summary>
        public readonly static TimeSpan DefaultInterval = 1.Seconds();

        /// <summary>
        /// Occurs when the timer elapses.
        /// </summary>
        public event ElapsedEventHandler Update;

        [field: NonSerialized]
        protected Timer timer;

        /// <summary>
        /// Initializes an instance of <see cref="BaseUpdate"/>.
        /// </summary>
        public BaseUpdate() : this(DefaultInterval) { }

        /// <summary>
        /// Initializes an instance of <see cref="BaseUpdate"/>.
        /// </summary>
        public BaseUpdate(TimeSpan interval) : base() => Reset(interval, false);

        void OnElapsed(object sender, ElapsedEventArgs e) => OnUpdate(e);

        void Unload()
        {
            if (timer != null)
            {
                timer.Enabled = false;
                timer.Elapsed -= OnElapsed;
                timer.Dispose();
            }
        }

        protected void Reset(TimeSpan interval, bool enabled = false)
        {
            Unload();
            timer = new Timer();
            timer.Elapsed += OnElapsed;
            timer.Interval = interval.TotalMilliseconds;
            timer.Enabled = enabled;
        }

        /// <summary>
        /// Occurs when the timer elapses.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUpdate(ElapsedEventArgs e) => Update?.Invoke(this, e);

        /// <inheritdoc/>
        protected override void OnUnmanagedDisposed()
        {
            base.OnUnmanagedDisposed();
            Unload();
        }
    }
}
