using System;
using System.Xml.Serialization;
using Imagin.Common.Input;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="ICheckable"/>.
    /// </summary>
    public class CheckableObject : ObjectBase, ICheckable
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> Checked;
        
        /// <summary>
        /// 
        /// </summary>
        public event CheckedEventHandler StateChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> Unchecked;

        /// <summary>
        /// 
        /// </summary>
        bool? _isChecked;
        /// <summary>
        /// 
        /// </summary>
        public virtual bool? IsChecked
        {
            get => _isChecked;
            set
            {
                if (Property.Set(this, ref _isChecked, value) && value != null)
                {
                    if (value.Value)
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
        public CheckableObject() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isChecked"></param>
        public CheckableObject(bool isChecked = false) => IsChecked = isChecked;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _isChecked?.ToString() ?? "Indeterminate";

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnChecked()
        {
            Checked?.Invoke(this, new EventArgs());
            OnStateChanged(true);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnIndeterminate() => OnStateChanged(null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="State"></param>
        protected virtual void OnStateChanged(bool? State) => StateChanged?.Invoke(this, new CheckedEventArgs(State));

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnUnchecked()
        {
            Unchecked?.Invoke(this, new EventArgs());
            OnStateChanged(false);
        }
    }
}
