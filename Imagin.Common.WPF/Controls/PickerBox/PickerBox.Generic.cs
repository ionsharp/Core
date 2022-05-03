using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Controls
{
    public abstract class PickerBox<T, Window> : PickerBox where Window : PickerWindow
    {
        public event EventHandler<EventArgs<T>> ValueChanged;

        protected abstract T DefaultValue { get; }

        public PickerBox() : base() { }

        protected virtual void OnValueChanged(Value<T> input) => ValueChanged?.Invoke(this, new EventArgs<T>(input.New));

        protected abstract T GetValue();

        protected abstract void SetValue(T i);

        public override bool? ShowDialog()
        {
            var window = typeof(Window).Create<Window>();
            window.Title = Title;
            window.Value = GetValue();

            var result = window.ShowDialog();
            if (XWindow.GetResult(window) == 0)
                SetValue((T)window.Value);

            return result;
        }
    }
}