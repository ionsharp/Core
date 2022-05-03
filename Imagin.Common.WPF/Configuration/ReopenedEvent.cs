using System;
using System.Collections.Generic;

namespace Imagin.Common.Configuration
{
    public delegate void ReopenedEventHandler(SingleApplication sender, ReopenedEventArgs e);

    public class ReopenedEventArgs : EventArgs
    {
        public readonly IEnumerable<string> Arguments;

        public ReopenedEventArgs(IEnumerable<string> arguments) : base()
        {
            Arguments = arguments;
        }
    }
}