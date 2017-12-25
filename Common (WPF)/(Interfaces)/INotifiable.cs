using System.Timers;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an object capable of raising notifications.
    /// </summary>
    public interface INotifiable
    {
        /// <summary>
        /// Occurs when <see cref="Timer"/> elapses.
        /// </summary>
        event ElapsedEventHandler Notified;

        /// <summary>
        /// Gets or sets whether or not to enable notifications.
        /// </summary>
        bool Enabled
        {
            get; set;
        }

        /// <summary>
        /// The period of time (in milliseconds) between notifications.
        /// </summary>
        double Interval
        {
            get; set;
        }
    }
}
