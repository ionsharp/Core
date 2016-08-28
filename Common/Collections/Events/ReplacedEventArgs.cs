using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagin.Common.Collections.Events
{
    public class ReplacedEventArgs<T> : EventArgs
    {
        public IEnumerable<T> OldItems;
        public IEnumerable<T> NewItems;
        public ReplacedEventArgs(IEnumerable<T> Old, IEnumerable<T> New)
        {
            this.OldItems = Old;
            this.NewItems = New;
        }
    }
}
