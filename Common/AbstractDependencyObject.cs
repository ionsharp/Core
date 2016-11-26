using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagin.Common.Attributes;
using System.ComponentModel;
using System.Windows;
using System.Xml.Serialization;

namespace Imagin.Common
{
    public class AbstractDependencyObject : DependencyObject
    {
        #region AbstractDependencyObject

        public AbstractDependencyObject() : base()
        {
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
