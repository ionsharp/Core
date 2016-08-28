using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagin.Common.Collections.Events
{
    public class RemovedEventArgs<T> : EventArgs
    {
        public IEnumerable<T> OldItems;
        public RemovedEventArgs(IEnumerable<T> Old)
        {
            this.OldItems = Old;
        }
    }
}
