using System;

namespace Imagin.Common.Input
{
    public delegate void LockedEventHandler(object sender, LockedEventArgs e);

    public class LockedEventArgs : EventArgs
    {
        public readonly bool IsLocked;

        public LockedEventArgs(bool isLocked) : base() => IsLocked = isLocked;
    }
}