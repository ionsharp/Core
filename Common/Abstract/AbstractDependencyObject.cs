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
    /// <summary>
    /// 
    /// </summary>
    public class AbstractDependencyObject : DependencyObject
    {
        #region AbstractDependencyObject

        /// <summary>
        /// 
        /// </summary>
        public AbstractDependencyObject() : base()
        {
        }

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
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
