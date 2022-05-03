using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    [Extends(typeof(Canvas))]
    public static class XCanvas
    {
        #region CanDrag

        /// <summary>
        /// Gets or sets if children can be dragged. Supports <see cref="ILock"/>.
        /// </summary>
        public static readonly DependencyProperty CanDragProperty = DependencyProperty.RegisterAttached("CanDrag", typeof(bool), typeof(XCanvas), new FrameworkPropertyMetadata(false, OnCanDragChanged));
        public static bool GetCanDrag(Canvas i) => (bool)i.GetValue(CanDragProperty);
        public static void SetCanDrag(Canvas i, bool input) => i.SetValue(CanDragProperty, input);
        static void OnCanDragChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Canvas canvas)
                canvas.RegisterHandlerAttached((bool)e.NewValue, CanDragProperty, CanDrag_Loaded, CanDrag_Unloaded);
        }

        //...

        static FrameworkElement GetTarget(FrameworkElement input)
        {
            DependencyObject parent = input;
            while (parent != null)
            {
                var nextParent = VisualTreeHelper.GetParent(parent);
                if (nextParent is Canvas)
                    break;

                parent = nextParent;
            }
            return parent as FrameworkElement;
        }

        //...

        static void CanDrag_Loaded(Canvas i)
        {
            i.MouseDown
                += CanDrag_MouseDown;
            i.MouseMove
                += CanDrag_MouseMove;
            i.MouseUp
                += CanDrag_MouseUp;
        }

        static void CanDrag_Unloaded(Canvas i)
        {
            i.MouseDown
                -= CanDrag_MouseDown;
            i.MouseMove
                -= CanDrag_MouseMove;
            i.MouseUp
                -= CanDrag_MouseUp;
        }

        //...

        static void CanDrag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                if (GetDragButton(canvas) == e.ChangedButton)
                {
                    var startPoint = e.GetPosition(canvas);
                    if (VisualTreeHelper.HitTest(canvas, startPoint)?.VisualHit is FrameworkElement element)
                    {
                        var dragTarget = GetTarget(element);
                        if (dragTarget != null)
                        {
                            var item = dragTarget.DataContext;
                            if (item is ILock lockableTarget)
                            {
                                if (lockableTarget.IsLocked)
                                    return;
                            }

                            SetDragOrigin
                                (canvas, new Point(Canvas.GetLeft(dragTarget), Canvas.GetTop(dragTarget)));
                            SetDragStart
                                (canvas, startPoint);
                            SetDragTarget
                                (canvas, dragTarget);
                            
                            dragTarget.CaptureMouse();
                        }
                    }
                }
            }
        }

        static void CanDrag_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                if (GetDragStart(canvas) != null)
                {
                    var current = e.GetPosition(canvas);
                    var offset = new Point(GetDragTarget(canvas).RenderTransform.Value.OffsetX, GetDragTarget(canvas).RenderTransform.Value.OffsetY);

                    var vector = current - GetDragStart(canvas);
                    if (vector != null)
                    {
                        var result = vector.Value.BoundSize(GetDragOrigin(canvas), offset, new Size(canvas.ActualWidth, canvas.ActualHeight), new Size(GetDragTarget(canvas).ActualWidth, GetDragTarget(canvas).ActualHeight), GetDragSnap(canvas));
                        Canvas.SetLeft
                            (GetDragTarget(canvas), result.X);
                        Canvas.SetTop
                            (GetDragTarget(canvas), result.Y);
                    }
                }
            }
        }

        static void CanDrag_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                SetDragOrigin
                    (canvas, null);
                SetDragStart
                    (canvas, null);

                if (GetDragTarget(canvas) != null)
                {
                    GetDragTarget(canvas).ReleaseMouseCapture();
                    SetDragTarget(canvas, null);
                }
            }
        }

        #endregion

        #region CanSelect

        /// <summary>
        /// Gets or sets if children can be selected. Supports <see cref="ISelect"/>.
        /// </summary>
        public static readonly DependencyProperty CanSelectProperty = DependencyProperty.RegisterAttached("CanSelect", typeof(bool), typeof(XCanvas), new FrameworkPropertyMetadata(false, OnCanSelectChanged));
        public static bool GetCanSelect(Canvas i) => (bool)i.GetValue(CanSelectProperty);
        public static void SetCanSelect(Canvas i, bool input) => i.SetValue(CanSelectProperty, input);
        static void OnCanSelectChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Canvas canvas)
                canvas.RegisterHandlerAttached((bool)e.NewValue, CanSelectProperty, i => i.MouseDown += CanSelect_MouseDown, i => i.MouseDown -= CanSelect_MouseDown);
        }

        static void CanSelect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                if (GetSelectionButton(canvas) == e.ChangedButton)
                {
                    if (VisualTreeHelper.HitTest(canvas, e.GetPosition(canvas))?.VisualHit is FrameworkElement frameworkElement)
                    {
                        var dragTarget = GetTarget(frameworkElement);
                        if (dragTarget != null)
                        {
                            foreach (FrameworkElement i in canvas.Children)
                            {
                                if (i.DataContext is ISelect select)
                                {
                                    select.IsSelected = ReferenceEquals(i, dragTarget);
                                    Canvas.SetZIndex(i, select.IsSelected ? 1 : 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region DragButton

        public static readonly DependencyProperty DragButtonProperty = DependencyProperty.RegisterAttached("DragButton", typeof(MouseButton), typeof(XCanvas), new FrameworkPropertyMetadata(MouseButton.Left));
        public static MouseButton GetDragButton(Canvas i) => (MouseButton)i.GetValue(DragButtonProperty);
        public static void SetDragButton(Canvas i, MouseButton input) => i.SetValue(DragButtonProperty, input);

        #endregion

        #region (private) DragOrigin

        static readonly DependencyProperty DragOriginProperty = DependencyProperty.RegisterAttached("DragOrigin", typeof(Point?), typeof(XCanvas), new FrameworkPropertyMetadata(null));
        static Point? GetDragOrigin(Canvas i) => (Point?)i.GetValue(DragOriginProperty);
        static void SetDragOrigin(Canvas i, Point? input) => i.SetValue(DragOriginProperty, input);

        #endregion

        #region DragSnap

        public static readonly DependencyProperty DragSnapProperty = DependencyProperty.RegisterAttached("DragSnap", typeof(double), typeof(XCanvas), new FrameworkPropertyMetadata(16.0));
        public static double GetDragSnap(Canvas i) => (double)i.GetValue(DragSnapProperty);
        public static void SetDragSnap(Canvas i, double input) => i.SetValue(DragSnapProperty, input);

        #endregion

        #region (private) DragStart

        static readonly DependencyProperty DragStartProperty = DependencyProperty.RegisterAttached("DragStart", typeof(Point?), typeof(XCanvas), new FrameworkPropertyMetadata(null));
        static Point? GetDragStart(Canvas i) => (Point?)i.GetValue(DragStartProperty);
        static void SetDragStart(Canvas i, Point? input) => i.SetValue(DragStartProperty, input);

        #endregion

        #region (private) DragTarget

        static readonly DependencyProperty DragTargetProperty = DependencyProperty.RegisterAttached("DragTarget", typeof(FrameworkElement), typeof(XCanvas), new FrameworkPropertyMetadata(null));
        static FrameworkElement GetDragTarget(Canvas i) => (FrameworkElement)i.GetValue(DragTargetProperty);
        static void SetDragTarget(Canvas i, FrameworkElement input) => i.SetValue(DragTargetProperty, input);

        #endregion

        #region SelectionButton

        public static readonly DependencyProperty SelectionButtonProperty = DependencyProperty.RegisterAttached("SelectionButton", typeof(MouseButton), typeof(XCanvas), new FrameworkPropertyMetadata(MouseButton.Left));
        public static MouseButton GetSelectionButton(Canvas i) => (MouseButton)i.GetValue(SelectionButtonProperty);
        public static void SetSelectionButton(Canvas i, MouseButton input) => i.SetValue(SelectionButtonProperty, input);

        #endregion

        public static void Set(UIElement input, Point point) { Canvas.SetLeft(input, point.X); Canvas.SetTop(input, point.Y); }
    }
}