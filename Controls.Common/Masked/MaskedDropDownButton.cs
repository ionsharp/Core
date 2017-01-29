using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class MaskedDropDownButton : MaskedToggleButton
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(MaskedDropDownButton), new UIPropertyMetadata(null, OnDropDownChanged));
        /// <summary>
        /// 
        /// </summary>
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
        static void OnDropDownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<MaskedDropDownButton>().OnDropDownChanged((ContextMenu)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DropDownDataContextProperty = DependencyProperty.Register("DropDownDataContext", typeof(object), typeof(MaskedDropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDropDownDataContextChanged));
        /// <summary>
        /// 
        /// </summary>
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
        static void OnDropDownDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<MaskedDropDownButton>().OnDropDownDataContextChanged(e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public MaskedDropDownButton()
        {
            DefaultStyleKey = typeof(MaskedDropDownButton);
            SetCurrentValue(ContentMarginProperty, new Thickness(5, 0, 0, 0));

            this.SetBinding(MaskedDropDownButton.IsCheckedProperty, new Binding("DropDown.IsOpen")
            {
                Source = this
            });
            this.SetBinding(MaskedDropDownButton.DropDownDataContextProperty, new Binding("DropDown.DataContext")
            {
                Source = this
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (DropDown != null)
                DropDown.IsOpen = true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnDropDownChanged(ContextMenu Value)
        {
            if (Value != null)
            {
                Value.PlacementTarget = this;
                Value.Placement = PlacementMode.Bottom;

                if (DropDownDataContext != null)
                    Value.DataContext = DropDownDataContext;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnDropDownDataContextChanged(object Value)
        {
            if (DropDown != null)
                DropDown.DataContext = Value;
        }
    }
}
