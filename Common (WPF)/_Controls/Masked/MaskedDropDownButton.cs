using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Imagin.Common.Linq;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class MaskedDropDownButton : MaskedToggleButton
    {
        #region MaskedDropDownButton

        /// <summary>
        /// 
        /// </summary>
        public MaskedDropDownButton()
        {
            DefaultStyleKey = typeof(MaskedDropDownButton);
            SetCurrentValue(ContentMarginProperty, new Thickness(5, 0, 0, 0));

            Loaded += OnLoaded;
        }

        #endregion

        #region Methods

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnLoaded(e);
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
        protected override void OnDropDownChanged(ContextMenu Value)
        {
            if (Value != null)
            {
                Value.PlacementTarget = this;
                Value.Placement = PlacementMode.Bottom;

                BindingOperations.SetBinding(Value, ContextMenu.DataContextProperty, new Binding()
                {
                    Mode = BindingMode.OneWay,
                    Path = new PropertyPath("DropDownDataContext"),
                    Source = this
                });
                BindingOperations.SetBinding(Value, ContextMenu.IsOpenProperty, new Binding()
                {
                    Path = new PropertyPath("IsChecked"),
                    Source = this
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnDropDownDataContextChanged(object Value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            SetCurrentValue(DropDownDataContextProperty, DataContext);
        }

        #endregion
    }
}
