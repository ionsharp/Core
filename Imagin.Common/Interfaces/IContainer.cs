using System.Collections.Generic;

namespace Imagin.Common
{
    public interface IContainer<T>
    {
        IList<T> Items { get; }
    }
}