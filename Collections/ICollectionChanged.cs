using Imagin.Core.Input;
using System.Collections;
using System.Collections.Specialized;

namespace Imagin.Core.Collections;

/// <summary>
/// Represents a non-generic collection of objects that can be individually accessed by index, and notifies listeners of dynamic changes, such as when an item is added and removed or the whole list is cleared.
/// </summary>
/// <remarks>
/// Implements <see cref="IList"/> and <see cref="INotifyCollectionChanged"/>.
/// </remarks>
public interface ICollectionChanged : IList, INotifyCollectionChanged
{
    event CancelEventHandler<object> Removing;
}