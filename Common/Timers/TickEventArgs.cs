using System;

namespace Imagin.Common.Timers
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TickEventHandler(object sender, TickEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class TickEventArgs : EventArgs
    {
        readonly TimeSpan elapsed;
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Elapsed
        {
            get
            {
                return elapsed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TickEventArgs(TimeSpan Elapsed) : base()
        {
            elapsed = Elapsed;
        }
    }
}
