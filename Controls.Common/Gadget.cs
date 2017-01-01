using System;
using System.ComponentModel;
using System.Windows;

namespace Imagin.Controls.Common
{
    public class Gadget : Window
    {
        #region Gadget

        public Gadget() : base()
        {
            this.DefaultStyleKey = typeof(Gadget);
        }

        #endregion

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
    }
}
