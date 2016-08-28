using System;

namespace Imagin.Common.Events
{
    public class ObjectEventArgs : EventArgs
    {
        public object Object = null;

        public ObjectEventArgs(object Object)
        {
            this.Object = Object;
        }
    }
}
