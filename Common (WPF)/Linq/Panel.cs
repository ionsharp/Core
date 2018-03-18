using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class PanelExtensions
    {

        #region HorizontalContentAlignment

        /// <summary>
        /// Applies <see cref="HorizontalAlignment"/> to all children except those that define <see cref="FrameworkElementExtensions.HorizontalAlignmentProperty"/>.
        /// </summary>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(PanelExtensions), new FrameworkPropertyMetadata(HorizontalAlignment.Left, OnHorizontalContentAlignmentChanged));
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
            var panel = sender as Panel;
            var value = GetHorizontalContentAlignment(panel);

            for (int i = 0, Count = panel.Children.Count; i < Count; i++)
            {
                var element = panel.Children[i].As<FrameworkElement>();
                if (FrameworkElementExtensions.GetHorizontalAlignment(element) == null)
                    element.HorizontalAlignment = value;
            }
        }

        #endregion

        #region Spacing

        /// <summary>
        /// Applies <see cref="Thickness"/> to all children except those that define <see cref="FrameworkElementExtensions.MarginProperty"/>.
        /// </summary>
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.RegisterAttached("Spacing", typeof(Thickness), typeof(PanelExtensions), new FrameworkPropertyMetadata(default(Thickness), OnSpacingChanged));
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
            var panel = sender as Panel;
            var value = GetSpacing(panel);

            var tf = GetTrimFirst(panel);
            var tl = GetTrimLast(panel);

            for (int i = 0, Count = panel.Children.Count; i < Count; i++)
            {
                var element = panel.Children[i] as FrameworkElement;
                if ((i == 0 && tf) || (i == (Count - 1) && tl))
                {
                    element.Margin = new Thickness(0);
                }
                else if (FrameworkElementExtensions.GetMargin(element) == null)
                    element.Margin = value;
            }
        }

        #endregion

        #region TrimFirst

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TrimFirstProperty = DependencyProperty.RegisterAttached("TrimFirst", typeof(bool), typeof(PanelExtensions), new FrameworkPropertyMetadata(false, OnSpacingChanged));
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
        public static readonly DependencyProperty TrimLastProperty = DependencyProperty.RegisterAttached("TrimLast", typeof(bool), typeof(PanelExtensions), new FrameworkPropertyMetadata(false, OnSpacingChanged));
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
        /// Applies <see cref="VerticalAlignment"/> to all children except those that define <see cref="FrameworkElementExtensions.VerticalAlignmentProperty"/>.
        /// </summary>
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.RegisterAttached("VerticalContentAlignment", typeof(VerticalAlignment), typeof(PanelExtensions), new FrameworkPropertyMetadata(VerticalAlignment.Top, OnVerticalContentAlignmentChanged));
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
            var panel = sender as Panel;
            var value = GetVerticalContentAlignment(panel);

            for (int i = 0, Count = panel.Children.Count; i < Count; i++)
            {
                var element = panel.Children[i].As<FrameworkElement>();
                if (FrameworkElementExtensions.GetVerticalAlignment(element) == null)
                    element.VerticalAlignment = value;
            }
        }

        #endregion
    }
}