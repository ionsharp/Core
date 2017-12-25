using Imagin.Common.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class SliderExtensions
    {
        #region VerticalContentAlignment

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ThumbToolTipValueConverterParameterProperty = DependencyProperty.RegisterAttached("VerticalContentAlignment", typeof(VerticalAlignment), typeof(PanelExtensions), new PropertyMetadata(VerticalAlignment.Top, OnVerticalContentAlignmentChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetVerticalContentAlignment(Panel d, VerticalAlignment value)
        {
            d.SetValue(VerticalContentAlignmentProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static VerticalAlignment GetVerticalContentAlignment(Panel d)
        {
            return (VerticalAlignment)d.GetValue(VerticalContentAlignmentProperty);
        }
        static void OnVerticalContentAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Panel = sender as Panel;

            Panel.SizeChanged -= OnVerticalContentAlignmentUpdated;
            Panel.SizeChanged += OnVerticalContentAlignmentUpdated;

            OnVerticalContentAlignmentUpdated(Panel, null);
        }
        static void OnVerticalContentAlignmentUpdated(object sender, SizeChangedEventArgs e)
        {
            var p = sender as Panel;
            var a = GetVerticalContentAlignment(p);

            for (int i = 0, Count = p.Children.Count; i < Count; i++)
                p.Children[i].As<FrameworkElement>().VerticalAlignment = a;
        }

        #endregion
    }
}
