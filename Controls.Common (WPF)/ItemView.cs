using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemView : ViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemContainerStyleProperty = ItemsControl.ItemContainerStyleProperty.AddOwner(typeof(ItemView));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = ItemsControl.ItemTemplateProperty.AddOwner(typeof(ItemView));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty = WrapPanel.ItemWidthProperty.AddOwner(typeof(ItemView));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty = WrapPanel.ItemHeightProperty.AddOwner(typeof(ItemView));
        /// <summary>
        /// 
        /// </summary>
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
