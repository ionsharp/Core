using System;

namespace Imagin.Common.Input
{
    public delegate void CheckedEventHandler(object sender, CheckedEventArgs e);

    public class CheckedEventArgs : EventArgs
    {
        public readonly bool? State;

        public CheckedEventArgs(bool? state) : base() => State = state;
    }
}
