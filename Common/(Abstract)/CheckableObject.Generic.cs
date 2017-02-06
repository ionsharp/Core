using Imagin.Common.Input;
using System;
using System.Xml.Serialization;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class CheckableObject<TValue> : CheckableObject
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        protected TValue value;
        /// <summary>
        /// 
        /// </summary>
        public TValue Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public CheckableObject() : this(false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isChecked"></param>
        public CheckableObject(TValue value, bool isChecked = false) : this(isChecked)
        {
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isChecked"></param>
        public CheckableObject(bool isChecked = false) : base(isChecked)
        {
        }
    }
}
