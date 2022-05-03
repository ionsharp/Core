using System;

namespace Imagin.Common.Input.HotKeys
{
    ///<summary>
    /// The event arguments passed when a HotKeySet's OnHotKeysDownHold event is triggered.
    ///</summary>
    public sealed class HotKeyArgs : EventArgs
    {
        readonly DateTime time;
        ///<summary>
        /// Time when the event was triggered
        ///</summary>
        public DateTime Time
        {
            get
            {
                return time;
            }
        }

        ///<summary>
        /// Creates an instance of the HotKeyArgs.
        /// <param name="triggeredAt">Time when the event was triggered</param>
        ///</summary>
        public HotKeyArgs(DateTime TriggeredAt)
        {
            time = TriggeredAt;
        }
    }
}
