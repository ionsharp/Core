using System;
using System.ComponentModel;

namespace Imagin.Common
{
    [Serializable]
    public abstract class AbstractObject : INotifyPropertyChanged, ICloneable
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

        #region ICloneable

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        #region AbstractObject

        public AbstractObject()
        {
        }

        #endregion
    }
}
