using System.Collections.Generic;

namespace Imagin.Core;

public interface IContainer<T>
{
    IList<T> Items { get; }
}