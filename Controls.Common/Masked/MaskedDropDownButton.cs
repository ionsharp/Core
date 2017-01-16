using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class MaskedDropDownButton : MaskedToggleButton
    {
        #region Properties

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(MaskedDropDownButton), new UIPropertyMetadata(null, OnDropDownChanged));
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
        static void OnDropDownChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            MaskedDropDownButton MaskedDropDownButton = (MaskedDropDownButton)Object;
            if (MaskedDropDownButton.DropDown != null)
            {
                MaskedDropDownButton.DropDown.PlacementTarget = MaskedDropDownButton;
                MaskedDropDownButton.DropDown.Placement = PlacementMode.Bottom;
                if (MaskedDropDownButton.DropDownDataContext != null)
                    MaskedDropDownButton.DropDown.DataContext = MaskedDropDownButton.DropDownDataContext;
            }
        }

        public static DependencyProperty DropDownDataContextProperty = DependencyProperty.Register("DropDownDataContext", typeof(object), typeof(MaskedDropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDropDownDataContextChanged));
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
            MaskedDropDownButton MaskedDropDownButton = (MaskedDropDownButton)Object;
            if (MaskedDropDownButton.DropDown != null)
                MaskedDropDownButton.DropDown.DataContext = MaskedDropDownButton.DropDownDataContext;
        }

        #endregion

        #region MaskedDropDownButton

        public MaskedDropDownButton()
        {
            this.DefaultStyleKey = typeof(MaskedDropDownButton);
            this.ContentMargin = new Thickness(5, 0, 0, 0);
            this.SetBinding(MaskedDropDownButton.IsCheckedProperty, new Binding("DropDown.IsOpen")
            {
                Source = this
            });
            this.SetBinding(MaskedDropDownButton.DropDownDataContextProperty, new Binding("DropDown.DataContext")
            {
                Source = this
            });
        }

        #endregion

        #region Methods

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (DropDown != null)
                DropDown.IsOpen = true;
        }

        #endregion
    }
}
