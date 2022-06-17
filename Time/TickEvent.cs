using System;

namespace Imagin.Core.Time
{
    public delegate void TickEventHandler(BaseTimer sender, TickEventArgs e);

    public class TickEventArgs : EventArgs
    {
        public readonly TimeSpan Elapsed;

        public TickEventArgs(TimeSpan elapsed)
        {
            Elapsed = elapsed;
        }
    }
}