using Imagin.Common.Extensions;
using Imagin.Controls.Common;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public abstract class BrushDialogBase<T> : BasicWindow where T : Brush
    {
        public static DependencyProperty FooterStyleProperty = DependencyProperty.Register("FooterStyle", typeof(Style), typeof(BrushDialogBase<T>), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style FooterStyle
        {
            get
            {
                return (Style)GetValue(FooterStyleProperty);
            }
            set
            {
                SetValue(FooterStyleProperty, value);
            }
        }

        protected abstract string DefaultTitle
        {
            get;
        }

        public Chip<T> Chip
        {
            get; set;
        }

        public T InitialValue
        {
            get; set;
        }

        public T Value
        {
            get; set;
        }

        public BrushDialogBase() : base()
        {
            DataContext = this;
            Title = DefaultTitle;
        }

        public BrushDialogBase(string title, T initialValue, Chip<T> chip = null) : this()
        {
            Title = title.IsNullOrEmpty() ? Title : title;
            Value = InitialValue = initialValue;
            Chip = chip;
        }

        protected abstract void OnRevert(object sender, RoutedEventArgs e);

        protected void OnCancel(object sender, RoutedEventArgs e)
        {
            Result = WindowResult.Cancel;
            Close();
        }

        protected void OnSave(object sender, RoutedEventArgs e)
        {
            Result = WindowResult.Ok;
            Close();
        }
    }
}
