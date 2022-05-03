using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(Panel))]
    public static class XPanel
    {
        #region HorizontalContentAlignment

        /// <summary>
        /// Applies <see cref="HorizontalAlignment"/> to all children except those that define <see cref="XElement.HorizontalAlignmentProperty"/>.
        /// </summary>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(XPanel), new FrameworkPropertyMetadata(HorizontalAlignment.Left, OnHorizontalContentAlignmentChanged));
        public static void SetHorizontalContentAlignment(Panel i, HorizontalAlignment input) => i.SetValue(HorizontalContentAlignmentProperty, input);
        public static HorizontalAlignment GetHorizontalContentAlignment(Panel i) => (HorizontalAlignment)i.GetValue(HorizontalContentAlignmentProperty);
        static void OnHorizontalContentAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Panel panel)
                panel.RegisterHandlerAttached(true, HorizontalContentAlignmentProperty, HorizontalContentAlignment_Loaded, null);
        }

        static void HorizontalContentAlignment_Loaded(Panel panel)
        {
            var value = GetHorizontalContentAlignment(panel);
            for (int i = 0, count = panel.Children.Count; i < count; i++)
            {
                var j = panel.Children[i] as FrameworkElement;
                if (XElement.GetOverrideHorizontalAlignment(j) == null)
                    j.HorizontalAlignment = value;
            }
        }

        #endregion

        #region VerticalContentAlignment

        /// <summary>
        /// Applies <see cref="VerticalAlignment"/> to all children except those that define <see cref="XElement.VerticalAlignmentProperty"/>.
        /// </summary>
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.RegisterAttached("VerticalContentAlignment", typeof(VerticalAlignment), typeof(XPanel), new FrameworkPropertyMetadata(VerticalAlignment.Top, OnVerticalContentAlignmentChanged));
        public static void SetVerticalContentAlignment(Panel i, VerticalAlignment input) => i.SetValue(VerticalContentAlignmentProperty, input);
        public static VerticalAlignment GetVerticalContentAlignment(Panel i) => (VerticalAlignment)i.GetValue(VerticalContentAlignmentProperty);
        static void OnVerticalContentAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Panel panel)
                panel.RegisterHandlerAttached(true, VerticalContentAlignmentProperty, VerticalContentAlignment_Loaded, null);
        }

        static void VerticalContentAlignment_Loaded(Panel panel)
        {
            var value = GetVerticalContentAlignment(panel);
            for (int i = 0, Count = panel.Children.Count; i < Count; i++)
            {
                var j = panel.Children[i] as FrameworkElement;
                if (XElement.GetOverrideVerticalAlignment(j) == null)
                    j.VerticalAlignment = value;
            }
        }

        #endregion

        #region Spacing

        /// <summary>
        /// Applies <see cref="Thickness"/> to all children except those that define <see cref="XElement.MarginProperty"/>.
        /// </summary>
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.RegisterAttached("Spacing", typeof(Thickness), typeof(XPanel), new FrameworkPropertyMetadata(default(Thickness), OnSpacingChanged));
        public static void SetSpacing(Panel i, Thickness input) => i.SetValue(SpacingProperty, input);
        public static Thickness GetSpacing(Panel i) => (Thickness)i.GetValue(SpacingProperty);
        static void OnSpacingChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Panel panel)
                panel.RegisterHandlerAttached(true, SpacingProperty, Spacing_Loaded, null);
        }

        static void Spacing_Loaded(Panel panel)
        {
            var spacing 
                = GetSpacing(panel);
            var except 
                = GetSpacingExcept(panel);

            var eFirst 
                = except.HasFlag(SpacingExceptions.First);
            var eLast
                = except.HasFlag(SpacingExceptions.Last);

            for (int i = 0, Count = panel.Children.Count; i < Count; i++)
            {
                var j = panel.Children[i].As<FrameworkElement>();
                if (XElement.GetOverrideMargin(j) == null)
                {
                    j.Margin
                        = (i == 0 && eFirst) || (i == (Count - 1) && eLast)
                        ? new Thickness(0)
                        : j.Margin = spacing;
                }
            }
        }

        #endregion

        #region SpacingExcept

        public static readonly DependencyProperty SpacingExceptProperty = DependencyProperty.RegisterAttached("SpacingExcept", typeof(SpacingExceptions), typeof(XPanel), new FrameworkPropertyMetadata(SpacingExceptions.None, OnSpacingChanged));
        public static void SetSpacingExcept(Panel i, SpacingExceptions input) => i.SetValue(SpacingExceptProperty, input);
        public static SpacingExceptions GetSpacingExcept(Panel i) => (SpacingExceptions)i.GetValue(SpacingExceptProperty);

        #endregion

        #region (enum) SpacingExceptions

        [Flags]
        public enum SpacingExceptions { None = 0, First = 1, Last = 2 }

        #endregion
    }
}