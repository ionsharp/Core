using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class DropDownButton : Button
    {
        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(DropDownButton), new UIPropertyMetadata(null));
        public ContextMenu DropDown
        {
            get
            {
                return (ContextMenu)GetValue(DropDownProperty);
            }
            set
            {
                SetValue(DropDownProperty, value);
            }
        }

        public static DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(DropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

        public static DependencyProperty DropDownDataContextProperty = DependencyProperty.Register("DropDownDataContext", typeof(object), typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDropDownDataContextChanged));
        public object DropDownDataContext
        {
            get
            {
                return (object)GetValue(DropDownDataContextProperty);
            }
            set
            {
                SetValue(DropDownDataContextProperty, value);
            }
        }
        static void OnDropDownDataContextChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            DropDownButton DropDownButton = Object as DropDownButton;
            if (DropDownButton.DropDown != null && DropDownButton.DropDownDataContext != null)
                DropDownButton.DropDown.DataContext = DropDownButton.DropDownDataContext;
        }

        #region DropDownButton

        public DropDownButton()
        {
            this.DefaultStyleKey = typeof(DropDownButton);
            Binding Binding = new Binding("DropDown.IsOpen");
            Binding.Source = this;
            this.SetBinding(DropDownButton.IsCheckedProperty, Binding);
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
        }

        #endregion

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (DropDown != null)
            {
                DropDown.PlacementTarget = this;
                DropDown.Placement = PlacementMode.Bottom;
                DropDown.IsOpen = true;
            }
        }
    }
}
