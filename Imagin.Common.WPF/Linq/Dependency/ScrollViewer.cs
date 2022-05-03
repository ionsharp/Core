using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(ScrollViewer))]
    public static class XScrollViewer
    {
        #region CanZoom

        public static readonly DependencyProperty CanZoomProperty = DependencyProperty.RegisterAttached("CanZoom", typeof(bool), typeof(XScrollViewer), new FrameworkPropertyMetadata(false));
        public static bool GetCanZoom(ScrollViewer i) => (bool)i.GetValue(CanZoomProperty);
        public static void SetCanZoom(ScrollViewer i, bool input) => i.SetValue(CanZoomProperty, input);

        #endregion

        #region Overlap

        public static readonly DependencyProperty OverlapProperty = DependencyProperty.RegisterAttached("Overlap", typeof(bool), typeof(XScrollViewer), new FrameworkPropertyMetadata(false));
        public static bool GetOverlap(ScrollViewer i) => (bool)i.GetValue(OverlapProperty);
        public static void SetOverlap(ScrollViewer i, bool input) => i.SetValue(OverlapProperty, input);

        #endregion

        #region Zoom

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.RegisterAttached("Zoom", typeof(double), typeof(XScrollViewer), new FrameworkPropertyMetadata(1.0));
        public static double GetZoom(ScrollViewer i) => (double)i.GetValue(ZoomProperty);
        public static void SetZoom(ScrollViewer i, double input) => i.SetValue(ZoomProperty, input);

        #endregion
    }
}