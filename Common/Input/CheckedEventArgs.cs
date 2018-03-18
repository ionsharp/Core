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
        readonly bool? _state = null;
        /// <summary>
        /// 
        /// </summary>
        public bool? State => _state;

        /// <summary>
        /// 
        /// </summary>
        public CheckedEventArgs(bool? state) : base() => _state = state;
    }
}
