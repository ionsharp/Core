using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="IPropertyChanged"/>.
    /// </summary>
    public abstract class ObjectBase : object, IPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected bool SetValue<TValue>(ref TValue field, TValue value, [CallerMemberName] string propertyName = "") => Property.Set(this, ref field, value, propertyName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}