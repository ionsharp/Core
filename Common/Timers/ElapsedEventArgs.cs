using System;

namespace Imagin.Common.Timers
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ElapsedEventHandler(object sender, ElapsedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class ElapsedEventArgs : EventArgs
    {
        readonly DateTime signalTime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime SignalTime
        {
            get
            {
                return signalTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ElapsedEventArgs(DateTime SignalTime) : base()
        {
            signalTime = SignalTime;
        }
    }
}
