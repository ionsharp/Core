using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(Border))]
    public static class XBorder
    {
        #region Clip

        /// <summary>
        /// To do: Migrate <see cref="Imagin.Common.Controls.ClipBorder"/>.
        /// </summary>
        public static readonly DependencyProperty ClipProperty = DependencyProperty.RegisterAttached("Clip", typeof(bool), typeof(XBorder), new FrameworkPropertyMetadata(false, OnClipChanged));
        public static bool GetClip(Border i) => (bool)i.GetValue(ClipProperty);
        public static void SetClip(Border i, bool input) => i.SetValue(ClipProperty, input);
        static void OnClipChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Border border)
            {
            }
        }

        #endregion
    }
}