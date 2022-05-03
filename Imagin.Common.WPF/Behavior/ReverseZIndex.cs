using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Imagin.Common.Behavior
{
    public class ReverseZIndex : Behavior<Panel>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.LayoutUpdated += new EventHandler(OnLayoutUpdated);
        }

        protected void OnLayoutUpdated(object sender, EventArgs e)
        {
            int childCount = AssociatedObject.Children.Count;
            foreach (FrameworkElement element in AssociatedObject.Children)
            {
                element.SetValue(Panel.ZIndexProperty, childCount);
                childCount--;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.LayoutUpdated -= OnLayoutUpdated;
        }
    }
}