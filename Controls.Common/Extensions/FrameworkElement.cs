using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class FrameworkElementExtensions
    {
        #region Properties

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
