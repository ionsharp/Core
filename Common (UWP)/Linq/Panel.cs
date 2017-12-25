using Imagin.Common.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class PanelExtensions
    {
        #region HorizontalContentAlignment

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(PanelExtensions), new PropertyMetadata(HorizontalAlignment.Left, OnHorizontalContentAlignmentChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetHorizontalContentAlignment(Panel d, HorizontalAlignment value)
        {
            d.SetValue(HorizontalContentAlignmentProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static HorizontalAlignment GetHorizontalContentAlignment(Panel d)
        {
            return (HorizontalAlignment)d.GetValue(HorizontalContentAlignmentProperty);
        }
        static void OnHorizontalContentAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Panel = sender as Panel;

            Panel.SizeChanged -= OnHorizontalContentAlignmentUpdated;
            Panel.SizeChanged += OnHorizontalContentAlignmentUpdated;

            OnHorizontalContentAlignmentUpdated(Panel, null);
        }
        static void OnHorizontalContentAlignmentUpdated(object sender, SizeChangedEventArgs e)
        {
            var p = sender as Panel;
            var a = GetHorizontalContentAlignment(p);

            for (int i = 0, Count = p.Children.Count; i < Count; i++)
                p.Children[i].As<FrameworkElement>().HorizontalAlignment = a;
        }

        #endregion

        #region Spacing

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.RegisterAttached("Spacing", typeof(Thickness), typeof(PanelExtensions), new PropertyMetadata(default(Thickness), OnSpacingChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetSpacing(Panel d, Thickness value)
        {
            d.SetValue(SpacingProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Thickness GetSpacing(Panel d)
        {
            return (Thickness)d.GetValue(SpacingProperty);
        }
        static void OnSpacingChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Panel = sender as Panel;

            Panel.SizeChanged -= OnSpacingUpdated;
            Panel.SizeChanged += OnSpacingUpdated;

            OnSpacingUpdated(Panel, null);
        }
        static void OnSpacingUpdated(object sender, SizeChangedEventArgs e)
        {
            var p = sender as Panel;
            var s = GetSpacing(p);

            var tf = GetTrimFirst(p);
            var tl = GetTrimLast(p);

            for (int i = 0, Count = p.Children.Count; i < Count; i++)
            {
                var Element = p.Children[i] as FrameworkElement;
                if ((i == 0 && tf) || (i == (Count - 1) && tl))
                {
                    Element.Margin = new Thickness(0);
                    continue;
                }
                Element.Margin = s;
            }
        }

        #endregion

        #region TrimFirst

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TrimFirstProperty = DependencyProperty.RegisterAttached("TrimFirst", typeof(bool), typeof(PanelExtensions), new PropertyMetadata(false, OnSpacingChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetTrimFirst(Panel d, bool value)
        {
            d.SetValue(TrimFirstProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetTrimFirst(Panel d)
        {
            return (bool)d.GetValue(TrimFirstProperty);
        }

        #endregion

        #region TrimLast

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TrimLastProperty = DependencyProperty.RegisterAttached("TrimLast", typeof(bool), typeof(PanelExtensions), new PropertyMetadata(false, OnSpacingChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetTrimLast(Panel d, bool value)
        {
            d.SetValue(TrimLastProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetTrimLast(Panel d)
        {
            return (bool)d.GetValue(TrimLastProperty);
        }

        #endregion

        #region VerticalContentAlignment

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.RegisterAttached("VerticalContentAlignment", typeof(VerticalAlignment), typeof(PanelExtensions), new PropertyMetadata(VerticalAlignment.Top, OnVerticalContentAlignmentChanged));
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
