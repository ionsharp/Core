using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class DropDownButton : Button
    {
        #region DropDownButton

        public DropDownButton()
        {
            this.DefaultStyleKey = typeof(DropDownButton);

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
