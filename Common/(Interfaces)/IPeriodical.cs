using Imagin.Common.Timers;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an object that raises notifications over an indefinite period.
    /// </summary>
    public interface IPeriodical
    {
        /// <summary>
        /// Occurs when notifications are periodically raised.
        /// </summary>
        event ElapsedEventHandler Notified;
    }
}
