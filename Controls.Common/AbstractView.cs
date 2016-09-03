using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    public class AbstractView : ViewBase
    {
        public static readonly DependencyProperty ItemContainerStyleProperty = ItemsControl.ItemContainerStyleProperty.AddOwner(typeof(AbstractView));
        public Style ItemContainerStyle
        {
            get
            {
                return (Style)GetValue(ItemContainerStyleProperty);
            }
            set
            {
                SetValue(ItemContainerStyleProperty, value);
            }
        }

        public static readonly DependencyProperty ItemTemplateProperty = ItemsControl.ItemTemplateProperty.AddOwner(typeof(AbstractView));
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }
            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty ItemWidthProperty = WrapPanel.ItemWidthProperty.AddOwner(typeof(AbstractView));
        public double ItemWidth
        {
            get
            {
                return (double)GetValue(ItemWidthProperty);
            }
            set
            {
                SetValue(ItemWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ItemHeightProperty = WrapPanel.ItemHeightProperty.AddOwner(typeof(AbstractView));
        public double ItemHeight
        {
            get
            {
                return (double)GetValue(ItemHeightProperty);
            }
            set
            {
                SetValue(ItemHeightProperty, value);
            }
        }
    }
}
