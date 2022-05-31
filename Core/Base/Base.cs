using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Imagin.Core
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="IPropertyChanged"/>.
    /// </summary>
    [Serializable]
    public abstract class Base : object, IPropertyChanged
    {
        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}