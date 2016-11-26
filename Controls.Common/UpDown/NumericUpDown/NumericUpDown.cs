using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public abstract class NumericUpDown<T> : UpDown<T>
    {
        #region Properties

        public abstract Regex Expression
        {
            get;
        }

        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(T), typeof(NumericUpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public T Increment
        {
            get
            {
                return (T)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        #endregion

        #region NumericUpDown

        public NumericUpDown() : base()
        {
        }

        #endregion

        #region Methods

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            this.OnPreviewTextInput(e, this.Expression);
        }

        protected virtual void OnPreviewTextInput(TextCompositionEventArgs e, Regex Expression)
        {
            e.Handled = this.CaretIndex == 0 && e.Text == "-" ? this.Text.Contains("-") : (this.Expression.IsMatch(e.Text) ? false : true);
        }

        #endregion
    }
}
