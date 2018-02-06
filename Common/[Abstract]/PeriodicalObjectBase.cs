using Imagin.Common.Timers;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PeriodicalObjectBase : DisposableObject, IPeriodical
    {
        /// <summary>
        /// The default interval to use.
        /// </summary>
        public const double DefaultInterval = 1000d;

        /// <summary>
        /// Occurs when the timer elapses.
        /// </summary>
        public event ElapsedEventHandler Notified;

        /// <summary>
        /// 
        /// </summary>
        protected PeriodicalObjectBase() : base()
        {
        }

        /// <summary>
        /// Occurs when the timer elapses.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNotified(ElapsedEventArgs e)
        {
            Notified?.Invoke(this, e);
        }
    }
}
