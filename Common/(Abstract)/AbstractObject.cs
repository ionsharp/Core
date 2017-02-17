using System;
using System.ComponentModel;
using Imagin.Common.Extensions;

namespace Imagin.Common
{
    /// <summary>
    /// A base for abstract objects (implements INotifyPropertyChanged).
    /// </summary>
    [Serializable]
    public abstract class AbstractObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKind"></typeparam>
        /// <param name="Source"></param>
        /// <param name="NewValue"></param>
        /// <param name="Names"></param>
        protected virtual bool SetValue<TKind>(ref TKind Source, TKind NewValue, params string[] Notify)
        {
            //Set value if the new value is different from the old
            if (!Source.Equals(NewValue))
            {
                Source = NewValue;

                //Notify all applicable properties
                Notify?.ForEach(i => OnPropertyChanged(i));

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public AbstractObject()
        {
        }
    }
}
