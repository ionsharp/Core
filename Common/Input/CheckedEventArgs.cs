using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CheckedEventHandler(object sender, CheckedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class CheckedEventArgs : EventArgs
    {
        readonly bool? state = null;
        /// <summary>
        /// 
        /// </summary>
        public bool? State
        {
            get
            {
                return state;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CheckedEventArgs(bool? State) : base()
        {
            state = State;
        }
    }
}
