using System.ComponentModel;

namespace Imagin.Common
{
    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    /// <remarks>
    /// Implements <see cref="INotifyPropertyChanged"/>.
    /// </remarks>
    public interface IPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <param name="propertyName"></param>
        void OnPropertyChanged(string propertyName);
    }
}