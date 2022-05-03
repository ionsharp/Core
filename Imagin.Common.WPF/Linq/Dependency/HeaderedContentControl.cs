using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(HeaderedContentControl))]
    public static class XHeaderedContentControl
    {
        public static readonly ReferenceKey<ContentPresenter> ContentKey = new();

        public static readonly ReferenceKey<ContentPresenter> HeaderKey = new();

        #region Properties

        #region (RoutedEvent) HeaderClicked

        public static readonly RoutedEvent HeaderClickedEvent = EventManager.RegisterRoutedEvent("HeaderClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HeaderedContentControl));
        public static void AddHeaderClickedHandler(DependencyObject i, RoutedEventHandler handler)
        {
            if (i is UIElement j)
                j.AddHandler(HeaderClickedEvent, handler);
        }
        public static void RemoveHeaderClickedHandler(DependencyObject i, RoutedEventHandler handler)
        {
            if (i is UIElement j)
                j.RemoveHandler(HeaderClickedEvent, handler);
        }

        #endregion

        #region (RoutedEvent) HeaderDoubleClicked

        public static readonly RoutedEvent HeaderDoubleClickedEvent = EventManager.RegisterRoutedEvent("HeaderDoubleClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HeaderedContentControl));
        public static void AddHeaderDoubleClickedHandler(DependencyObject i, RoutedEventHandler handler)
        {
            if (i is UIElement j)
                j.AddHandler(HeaderDoubleClickedEvent, handler);
        }
        public static void RemoveHeaderDoubleClickedHandler(DependencyObject i, RoutedEventHandler handler)
        {
            if (i is UIElement j)
                j.RemoveHandler(HeaderDoubleClickedEvent, handler);
        }

        #endregion

        #region HeaderPadding

        public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.RegisterAttached("HeaderPadding", typeof(Thickness), typeof(XHeaderedContentControl), new FrameworkPropertyMetadata(default(Thickness)));
        public static Thickness GetHeaderPadding(HeaderedContentControl i)
            => (Thickness)i.GetValue(HeaderPaddingProperty);
        public static void SetHeaderPadding(HeaderedContentControl i, Thickness input)
            => i.SetValue(HeaderPaddingProperty, input);

        #endregion

        #region HeaderPlacement

        public static readonly DependencyProperty HeaderPlacementProperty = DependencyProperty.RegisterAttached("HeaderPlacement", typeof(TopBottom), typeof(XHeaderedContentControl), new FrameworkPropertyMetadata(TopBottom.Top));
        public static TopBottom GetHeaderPlacement(HeaderedContentControl i)
            => (TopBottom)i.GetValue(HeaderPlacementProperty);
        public static void SetHeaderPlacement(HeaderedContentControl i, TopBottom input)
            => i.SetValue(HeaderPlacementProperty, input);

        #endregion

        #region HeaderVisibility

        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.RegisterAttached("HeaderVisibility", typeof(Visibility), typeof(XHeaderedContentControl), new FrameworkPropertyMetadata(Visibility.Visible));
        public static Visibility GetHeaderVisibility(HeaderedContentControl i)
            => (Visibility)i.GetValue(HeaderVisibilityProperty);
        public static void SetHeaderVisibility(HeaderedContentControl i, Visibility input)
            => i.SetValue(HeaderVisibilityProperty, input);

        #endregion

        #region HorizontalHeaderAlignment

        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalHeaderAlignment", typeof(HorizontalAlignment), typeof(XHeaderedContentControl), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
        public static HorizontalAlignment GetHorizontalHeaderAlignment(HeaderedContentControl i)
            => (HorizontalAlignment)i.GetValue(HorizontalHeaderAlignmentProperty);
        public static void SetHorizontalHeaderAlignment(HeaderedContentControl i, HorizontalAlignment input)
            => i.SetValue(HorizontalHeaderAlignmentProperty, input);

        #endregion

        #region VerticalHeaderAlignment

        public static readonly DependencyProperty VerticalHeaderAlignmentProperty = DependencyProperty.RegisterAttached("VerticalHeaderAlignment", typeof(VerticalAlignment), typeof(XHeaderedContentControl), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));
        public static VerticalAlignment GetVerticalHeaderAlignment(HeaderedContentControl i)
            => (VerticalAlignment)i.GetValue(VerticalHeaderAlignmentProperty);
        public static void SetVerticalHeaderAlignment(HeaderedContentControl i, VerticalAlignment input)
            => i.SetValue(VerticalHeaderAlignmentProperty, input);

        #endregion

        #endregion

        #region XHeaderedContentControl

        static XHeaderedContentControl()
        {
            EventManager.RegisterClassHandler(typeof(HeaderedContentControl), HeaderedContentControl.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler(OnMouseLeftButtonDown), true);
        }

        static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is HeaderedContentControl control)
            {
                var header = e.OriginalSource.FindParent<ContentPresenter>(i => ReferenceEquals(XElement.GetName(i), HeaderKey));
                if (header != null)
                {
                    switch (e.ClickCount)
                    {
                        case 1:
                            control.RaiseEvent(new RoutedEventArgs(HeaderClickedEvent, control));
                            break;
                        case 2:
                            control.RaiseEvent(new RoutedEventArgs(HeaderDoubleClickedEvent, control));
                            break;
                    }
                }
            }
        }

        #endregion
    }
}