using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu : System.Windows.Controls.Menu
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AdjacentContentProperty = DependencyProperty.Register("AdjacentContent", typeof(UIElement), typeof(Menu), new PropertyMetadata(default(UIElement)));
        /// <summary>
        /// 
        /// </summary>
        public UIElement AdjacentContent
        {
            get
            {
                return (UIElement)GetValue(AdjacentContentProperty);
            }
            set
            {
                SetValue(AdjacentContentProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OpposingContentProperty = DependencyProperty.Register("OpposingContent", typeof(UIElement), typeof(Menu), new PropertyMetadata(default(UIElement)));
        /// <summary>
        /// 
        /// </summary>
        public UIElement OpposingContent
        {
            get
            {
                return (UIElement)GetValue(OpposingContentProperty);
            }
            set
            {
                SetValue(OpposingContentProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Menu() : base() => DefaultStyleKey = typeof(Menu);
    }
}
