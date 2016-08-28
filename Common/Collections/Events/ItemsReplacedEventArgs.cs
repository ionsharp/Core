using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagin.Common.Collections.Events
{
    public class ItemsReplacedEventArgs<T> : ReplacedEventArgs<T>
    {
        public ItemsReplacedEventArgs(IEnumerable<T> Old, IEnumerable<T> New) : base(Old, New) { }
    }
}
