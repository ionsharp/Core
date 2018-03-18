using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class FrameworkElementExtensions
    {
        #region Properties

        #region _ContextMenu

        /// <summary>
        /// 
        /// </summary>
        static readonly DependencyProperty _ContextMenuProperty = DependencyProperty.RegisterAttached("_ContextMenu", typeof(ContextMenu), typeof(FrameworkElementExtensions), new PropertyMetadata(default(ContextMenu)));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        static ContextMenu Get_ContextMenu(FrameworkElement d)
        {
            return (ContextMenu)d.GetValue(_ContextMenuProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        static void Set_ContextMenu(FrameworkElement d, ContextMenu value)
        {
            d.SetValue(_ContextMenuProperty, value);
        }

        #endregion

        #region Background

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(FrameworkElementExtensions), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Brush GetBackground(FrameworkElement d)
        {
            return (Brush)d.GetValue(BackgroundProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetBackground(FrameworkElement d, Brush value)
        {
            d.SetValue(BackgroundProperty, value);
        }

        #endregion

        #region BorderBrush

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.RegisterAttached("BorderBrush", typeof(Brush), typeof(FrameworkElementExtensions), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Brush GetBorderBrush(FrameworkElement d)
        {
            return (Brush)d.GetValue(BorderBrushProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetBorderBrush(FrameworkElement d, Brush value)
        {
            d.SetValue(BorderBrushProperty, value);
        }

        #endregion

        #region BorderThickness

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(FrameworkElementExtensions), new PropertyMetadata(default(Thickness)));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Thickness GetBorderThickness(FrameworkElement d)
        {
            return (Thickness)d.GetValue(BorderThicknessProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetBorderThickness(FrameworkElement d, Thickness value)
        {
            d.SetValue(BorderThicknessProperty, value);
        }

        #endregion

        #region CornerRadius

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(FrameworkElementExtensions), new PropertyMetadata(default(CornerRadius)));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static CornerRadius GetCornerRadius(FrameworkElement d)
        {
            return (CornerRadius)d.GetValue(CornerRadiusProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetCornerRadius(FrameworkElement d, CornerRadius value)
        {
            d.SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region EnableContextMenu

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EnableContextMenuProperty = DependencyProperty.RegisterAttached("EnableContextMenu", typeof(bool), typeof(FrameworkElementExtensions), new PropertyMetadata(true, OnEnableContextMenuChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetEnableContextMenu(FrameworkElement d)
        {
            return (bool)d.GetValue(EnableContextMenuProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetEnableContextMenu(FrameworkElement d, bool value)
        {
            d.SetValue(EnableContextMenuProperty, value);
        }
        static void OnEnableContextMenuChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element != null)
            {
                if ((bool)e.NewValue)
                {
                    Element.ContextMenu = Get_ContextMenu(Element);
                    Set_ContextMenu(Element, default(ContextMenu));
                }
                else
                {
                    Set_ContextMenu(Element, Element.ContextMenu);
                    Element.ContextMenu = null;
                }
            }
        }

        #endregion

        #region HorizontalAlignment

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalAlignment", typeof(HorizontalAlignment?), typeof(FrameworkElementExtensions), new FrameworkPropertyMetadata(null, OnHorizontalAlignmentChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetHorizontalAlignment(FrameworkElement d, HorizontalAlignment? value)
        {
            d.SetValue(HorizontalAlignmentProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static HorizontalAlignment? GetHorizontalAlignment(FrameworkElement d)
        {
            return (HorizontalAlignment?)d.GetValue(HorizontalAlignmentProperty);
        }
        static void OnHorizontalAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                (sender as FrameworkElement).SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, e.NewValue);
        }

        #endregion

        #region Margin

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof(Thickness?), typeof(FrameworkElementExtensions), new FrameworkPropertyMetadata(null, OnMarginChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetMargin(FrameworkElement d, Thickness? value)
        {
            d.SetValue(MarginProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Thickness? GetMargin(FrameworkElement d)
        {
            return (Thickness?)d.GetValue(MarginProperty);
        }
        static void OnMarginChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                (sender as FrameworkElement).SetCurrentValue(FrameworkElement.MarginProperty, e.NewValue);
        }

        #endregion

        #region VerticalAlignment

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.RegisterAttached("VerticalAlignment", typeof(VerticalAlignment?), typeof(FrameworkElementExtensions), new FrameworkPropertyMetadata(null, OnVerticalAlignmentChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetVerticalAlignment(FrameworkElement d, VerticalAlignment? value)
        {
            d.SetValue(VerticalAlignmentProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static VerticalAlignment? GetVerticalAlignment(FrameworkElement d)
        {
            return (VerticalAlignment?)d.GetValue(VerticalAlignmentProperty);
        }
        static void OnVerticalAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                (sender as FrameworkElement).SetCurrentValue(FrameworkElement.VerticalAlignmentProperty, e.NewValue);
        }

        #endregion

        #region IsDragMoveEnabled

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsDragMoveEnabledProperty = DependencyProperty.RegisterAttached("IsDragMoveEnabled", typeof(bool), typeof(FrameworkElementExtensions), new PropertyMetadata(false, OnIsDragMoveEnabledChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetIsDragMoveEnabled(FrameworkElement d)
        {
            return (bool)d.GetValue(IsDragMoveEnabledProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetIsDragMoveEnabled(FrameworkElement d, bool value)
        {
            d.SetValue(IsDragMoveEnabledProperty, value);
        }
        static void OnIsDragMoveEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Window = (sender as FrameworkElement).GetParent<Window>();
            if (Window != null && (bool)e.NewValue)
            {
                (sender as FrameworkElement).MouseDown += (a, b) =>
                {
                    if (b.LeftButton == MouseButtonState.Pressed)
                        Window.DragMove();
                };
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Helper method to determine if the given framework element has the mouse over it or not.
        /// </summary>
        /// <param name="element">The FrameworkElement to test for mouse containment.</param>
        /// <returns>True, if the mouse is over the FrameworkElement; false, otherwise.</returns>
        public static bool ContainsMouse(this FrameworkElement element)
        {
            var point = Mouse.GetPosition(element);
            return
            (
                point.X >= 0
                &&
                point.X <= element.ActualWidth
                &&
                point.Y >= 0
                &&
                point.Y <= element.ActualHeight
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="Element"></param>
        /// <returns></returns>
        public static Style FindStyle<TElement>(this TElement Element) where TElement : FrameworkElement
        {
            return (Style)Element.FindResource(typeof(TElement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="Element"></param>
        /// <param name="Style"></param>
        /// <returns></returns>
        public static bool TryFindStyle<TElement>(this TElement Element, out Style Style) where TElement : FrameworkElement
        {
            try
            {
                Style = Element.FindStyle();
                return true;
            }
            catch
            {
                Style = default(Style);
                return false;
            }
        }

        #endregion
    }
}
