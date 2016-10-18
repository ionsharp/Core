using System;
using System.ComponentModel;

namespace Imagin.Common
{
    /// <summary>
    /// A base for abstract objects (implements INotifyPropertyChanged).
    /// </summary>
    [Serializable]
    public abstract class AbstractObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region AbstractObject

        public AbstractObject()
        {
        }

        #endregion
    }
}
