using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class AdvancedImageDropDownButton : ImageButton
    {
        #region DependencyProperties

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(AdvancedImageDropDownButton), new UIPropertyMetadata(null, OnDropDownChanged));
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
            AdvancedImageDropDownButton a = (AdvancedImageDropDownButton)Object;
            if (a.DropDown != null)
            {
                a.DropDown.PlacementTarget = a;
                a.DropDown.Placement = PlacementMode.Bottom;
            }
        }

        public static DependencyProperty DropDownToolTipProperty = DependencyProperty.Register("DropDownToolTip", typeof(string), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string DropDownToolTip
        {
            get
            {
                return (string)GetValue(DropDownToolTipProperty);
            }
            set
            {
                SetValue(DropDownToolTipProperty, value);
            }
        }

        public static DependencyProperty DropDownVisibilityProperty = DependencyProperty.Register("DropDownVisibility", typeof(Visibility), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility DropDownVisibility
        {
            get
            {
                return (Visibility)GetValue(DropDownVisibilityProperty);
            }
            set
            {
                SetValue(DropDownVisibilityProperty, value);
            }
        }

        public static DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #endregion

        #region AdvancedImageDropDownButton

        public AdvancedImageDropDownButton()
        {
            this.DefaultStyleKey = typeof(AdvancedImageDropDownButton);

            this.SetBinding(AdvancedImageDropDownButton.IsCheckedProperty, new Binding("DropDown.IsOpen")
            {
                Source = this
            });
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
            this.Template.FindName("PART_Dropdown", this).As<ContentControl>().MouseLeftButtonDown += OnDropdownMouseLeftButtonDown;
        }

        #region Events

        #endregion

        #endregion
    }
}
