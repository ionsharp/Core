using System.ComponentModel;

namespace Imagin.Common
{
    /// <summary>
    /// Notifies clients that a property value has changed (implements <see cref="INotifyPropertyChanged"/>).
    /// </summary>
    public interface IPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <param name="propertyName"></param>
        void OnPropertyChanged(string propertyName);
    }
}
