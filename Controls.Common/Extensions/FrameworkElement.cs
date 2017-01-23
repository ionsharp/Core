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
    }
}
