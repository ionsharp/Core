using Imagin.Common.Input;
using System;

namespace Imagin.Common.ComponentModel
{
    public class CheckableObject<T> : AbstractObject, ICheckable
    {
        public event EventHandler<EventArgs<bool>> Checked;

        bool isChecked;
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
                OnChecked(value);
            }
        }

        T _value;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        protected virtual void OnChecked(bool Value)
        {
            if (Checked != null)
                Checked(this, new EventArgs<bool>(Value));
        }
        
        public CheckableObject() : base()
        {
        }

        public CheckableObject(T Value, bool IsChecked = false) : base()
        {
            this.Value = Value;
            this.IsChecked = IsChecked;
        }
    }
}
