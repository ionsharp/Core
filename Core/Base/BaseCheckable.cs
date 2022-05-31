using Imagin.Core.Input;
using System;

namespace Imagin.Core
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="ICheck"/>.
    /// </summary>
    public class BaseCheckable : Base, ICheck
    {
        public event EventHandler<EventArgs> Checked;
        
        public event CheckedEventHandler StateChanged;

        public event EventHandler<EventArgs> Unchecked;

        bool? _isChecked;
        public virtual bool? IsChecked
        {
            get => _isChecked;
            set
            {
                if (this.Change(ref _isChecked, value) && value != null)
                {
                    if (value.Value)
                    {
                        OnChecked();
                    }
                    else OnUnchecked();
                }
            }
        }

        public BaseCheckable() : base() { }

        public BaseCheckable(bool isChecked = false) => IsChecked = isChecked;

        public override string ToString() => _isChecked?.ToString() ?? "Indeterminate";

        protected virtual void OnChecked()
        {
            Checked?.Invoke(this, new EventArgs());
            OnStateChanged(true);
        }

        protected virtual void OnIndeterminate() => OnStateChanged(null);

        protected virtual void OnStateChanged(bool? State) => StateChanged?.Invoke(this, new CheckedEventArgs(State));

        protected virtual void OnUnchecked()
        {
            Unchecked?.Invoke(this, new EventArgs());
            OnStateChanged(false);
        }
    }
}