using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CheckableObjectBase : AbstractObject
    {
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized()]
        public event EventHandler<EventArgs> Checked;

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized()]
        public event EventHandler<EventArgs> Unchecked;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
            //return isChecked.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnChecked()
        {
            Checked?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnUnchecked()
        {
            Unchecked?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        public CheckableObjectBase() : base()
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TriCheckableObject : CheckableObjectBase, ICheckable
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        protected bool? isChecked;
        /// <summary>
        /// 
        /// </summary>
        public virtual bool? IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                if (SetValue(ref isChecked, value, "IsChecked") && value != null)
                {
                    if (value.Value)
                    {
                        OnChecked();
                    }
                    else OnUnchecked();
                }
            }
        }

        bool ICheckable.IsChecked
        {
            get
            {
                return isChecked ?? false;
            }
            set
            {
                SetValue(ref isChecked, value, "IsChecked");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TriCheckableObject() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isChecked"></param>
        public TriCheckableObject(bool? isChecked = false) : base()
        {
            IsChecked = isChecked;
        }
    }
}
