using Imagin.Common.Input;
using System;
using System.Xml.Serialization;

namespace Imagin.Common.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class CheckableObject : AbstractObject, ICheckable
    {
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized()]
        public event CheckedEventHandler Checked;

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        protected bool isChecked;
        /// <summary>
        /// 
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
                if (value) OnChecked();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return isChecked.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnChecked()
        {
            Checked?.Invoke(new CheckedEventArgs(this));
        }

        /// <summary>
        /// 
        /// </summary>
        public CheckableObject() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isChecked"></param>
        public CheckableObject(bool isChecked = false) : base()
        {
            IsChecked = isChecked;
        }
    }
}
