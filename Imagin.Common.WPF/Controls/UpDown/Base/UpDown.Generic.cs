using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class UpDown<T> : UpDown, IUpDown<T>
    {
        #region Properties

        protected readonly Handle handle = false;

        /// <summary>
        /// The absolute maximum value possible.
        /// </summary>
        public abstract T AbsoluteMaximum { get; }

        /// <summary>
        /// The absolute minimum value possible.
        /// </summary>
        public abstract T AbsoluteMinimum { get; }

        /// <summary>
        /// The default value.
        /// </summary>
        public abstract T DefaultValue { get; }

        public static readonly DependencyProperty<T, UpDown<T>> MaximumProperty = new(nameof(Maximum), new FrameworkPropertyMetadata(default(T), OnMaximumChanged, OnMaximumCoerced));
        public T Maximum
        {
            get => MaximumProperty.Get(this);
            set => MaximumProperty.Set(this, value);
        }
        static void OnMaximumChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UpDown<T>>().OnMaximumChanged(new Value<T>(e));
        static object OnMaximumCoerced(DependencyObject i, object Value) => i.As<UpDown<T>>().OnMaximumCoerced(Value);

        public static readonly DependencyProperty<T, UpDown<T>> MinimumProperty = new(nameof(Minimum), new FrameworkPropertyMetadata(default(T), OnMinimumChanged, OnMinimumCoerced));
        public T Minimum
        {
            get => MinimumProperty.Get(this);
            set => MinimumProperty.Set(this, value);
        }
        static void OnMinimumChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UpDown<T>>().OnMinimumChanged(new Value<T>(e));
        static object OnMinimumCoerced(DependencyObject i, object Value) => i.As<UpDown<T>>().OnMinimumCoerced(Value);

        public static readonly DependencyProperty<T, UpDown<T>> ValueProperty = new(nameof(Value), new FrameworkPropertyMetadata(default(T), OnValueChanged, OnValueCoerced));
        public T Value
        {
            get => ValueProperty.Get(this);
            set => ValueProperty.Set(this, value);
        }
        static void OnValueChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UpDown<T>>().OnValueChanged(new Value<T>(e));
        static object OnValueCoerced(DependencyObject i, object Value) => i.As<UpDown<T>>().OnValueCoerced(Value);

        #endregion

        #region UpDown

        public UpDown() : base()
        {
            SetCurrentValue(MaximumProperty.Property, AbsoluteMaximum);
            SetCurrentValue(MinimumProperty.Property, AbsoluteMinimum);
            SetCurrentValue(ValueProperty.Property, DefaultValue);
            OnValueChanged(new Value<T>(Value));
        }

        #endregion

        #region Methods

        #region Abstract

        protected abstract T GetValue(string Value);

        protected abstract object OnMaximumCoerced(object Value);

        protected abstract object OnMinimumCoerced(object Value);

        protected abstract object OnValueCoerced(object Value);

        protected abstract string ToString(T Value);

        #endregion

        #region Overrides

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
            handle.SafeInvoke(() =>
            {
                var i = ToString(Value);
                if (i != Text)
                    Text = i;
            });
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            handle.SafeInvoke(() => Value = GetValue(Text));
        }

        public sealed override void ValueToMaximum() => SetCurrentValue(ValueProperty.Property, Maximum);

        public sealed override void ValueToMinimum() => SetCurrentValue(ValueProperty.Property, Minimum);

        #endregion

        #region Virtual

        protected virtual void OnMaximumChanged(Value<T> input) { }

        protected virtual void OnMinimumChanged(Value<T> input) { }

        protected virtual void OnValueChanged(Value<T> input) => handle.SafeInvoke(() => SetText(ToString(input.New)));

        #endregion

        #endregion
    }
}