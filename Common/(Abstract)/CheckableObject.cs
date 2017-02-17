using System;
using System.Xml.Serialization;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class CheckableObject : CheckableObjectBase, ICheckable
    {
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
                if (SetValue(ref isChecked, value, "IsChecked"))
                {
                    if (value)
                    {
                        OnChecked();
                    }
                    else OnUnchecked();
                }
            }
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
