using Imagin.Common.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class NumericUpDown<T> : UpDown<T>
    {
        protected const string Comma = ",";

        protected const string Dash = "-";

        protected const string Period = ".";

        public abstract T DefaultIncrement { get; }

        public virtual Regex Expression => new("^[0-9]?$");

        public abstract bool IsRational { get; }

        public abstract bool IsSigned { get; }

        public static readonly DependencyProperty<T, NumericUpDown<T>> IncrementProperty = new(nameof(Increment), new FrameworkPropertyMetadata(default(T), OnIncrementChanged));
        public T Increment
        {
            get => IncrementProperty.Get(this);
            set => IncrementProperty.Set(this, value);
        }
        static void OnIncrementChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<NumericUpDown<T>>().OnIncrementChanged(new Value<T>(e));

        public static readonly DependencyProperty<string, NumericUpDown<T>> StringFormatProperty = new(nameof(StringFormat), new FrameworkPropertyMetadata(string.Empty, OnStringFormatChanged));
        public string StringFormat
        {
            get => StringFormatProperty.Get(this);
            set => StringFormatProperty.Set(this, value);
        }
        static void OnStringFormatChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<NumericUpDown<T>>().OnStringFormatChanged(new Value<string>(e));

        public NumericUpDown() : base() => SetCurrentValue(IncrementProperty.Property, DefaultIncrement);

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = e.Text switch
            {
                Dash => !(IsSigned && CaretIndex == 0 && !Text.Contains(Dash)),
                Period => !(!IsRational && !Text.Contains(Period)),
                _ => !Expression.IsMatch(e.Text),
            };
        }

        protected virtual void OnIncrementChanged(Value<T> input) { }

        protected virtual void OnStringFormatChanged(Value<string> input)
        {
            OnValueChanged(new Value<T>(default, Value));
        }
    }
}