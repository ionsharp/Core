using Imagin.Common.Linq;
using Imagin.Common.Analytics;
using Imagin.Common.Numbers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public sealed class SelectionPresenter : Presenter<ItemsControl>
    {
        ScrollViewer scrollViewer = null;

        //...

        readonly DoubleRegion Selection = new();

        readonly List<Rect> selections = new();

        //...

        bool isDragging = false;

        Rect previousSelection = new();

        Point startPoint;

        //...

        double scrollOffset
            => XItemsControl.GetDragScrollOffset(Control);

        double scrollOffsetMaximum
            => XItemsControl.GetDragScrollOffsetMaximum(Control);

        double scrollTolerance
            => XItemsControl.GetDragScrollTolerance(Control);

        SelectionMode selectionMode
        {
            get
            {
                bool result()
                {
                    if (Control is ListBox a)
                        return a.SelectionMode == System.Windows.Controls.SelectionMode.Single;

                    if (Control is DataGrid b)
                        return b.SelectionMode == DataGridSelectionMode.Single;

                    if (Control is TreeView c)
                        return XTreeView.GetSelectionMode(c) == Controls.SelectionMode.Single;

                    return false;
                }
                return result() ? Controls.SelectionMode.Single : Controls.SelectionMode.Multiple;
            }
        }

        //...

        public SelectionPresenter() : base() => Content = Selection;

        //...

        protected override object OnBackgroundCoerced(object input) => input is Brush i ? i : Brushes.Transparent;

        protected override void OnLoaded(Presenter<ItemsControl> i)
        {
            base.OnLoaded(i);
            scrollViewer = i.FindParent<ScrollViewer>() ?? throw new ParentNotFoundException<SelectionPresenter, ScrollViewer>();
        }

        //...

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (!XItemsControl.GetCanDragSelect(Control) || scrollViewer == null)
                return;

            if (e.ChangedButton == MouseButton.Left && selectionMode == SelectionMode.Multiple)
            {
                isDragging = true;

                Panel.SetZIndex(this, int.MaxValue);

                CaptureMouse();
                startPoint = e.GetPosition(this);
                if (!ModifierKeys.Control.Pressed() && !ModifierKeys.Shift.Pressed())
                    selections.Clear();

                previousSelection = new Rect();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDragging)
            {
                var selection = GetSelection(startPoint, e.GetPosition(this));
                //If still dragging after determining selection
                if (isDragging)
                {
                    //Update the visual selection
                    Selection.X = selection.X; Selection.Y = selection.Y;
                    Selection.Height = selection.Height; Selection.Width = selection.Width;

                    var tLeft
                        = new Point(Selection.TopLeft.X, Selection.TopLeft.Y);
                    var bRight
                        = new Point(Selection.BottomRight.X, Selection.BottomRight.Y);

                    //Select the items that lie below it
                    Select(Control, new Rect(tLeft, bRight));
                    //Scroll as mouse moves
                    Scroll(e.GetPosition(Control));
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.LeftButton == MouseButtonState.Released && isDragging)
            {
                var endPoint = e.GetPosition(this);
                isDragging = false;

                Panel.SetZIndex(this, 0);

                if (IsMouseCaptured)
                    ReleaseMouseCapture();

                if (!Try.Invoke(() => selections.Add(previousSelection)))
                    Try.Invoke(() => selections.Clear());

                Selection.X = 0; Selection.Y = 0; Selection.Height = 0; Selection.Width = 0;
                startPoint = default;
            }
        }

        //...

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == ContentProperty)
            {
                if (Content?.Equals(Selection) == false)
                    throw new ExternalChangeException<SelectionPresenter>(nameof(Content));
            }
        }

        //...

        Rect GetItemBounds(FrameworkElement i)
        {
            var topLeft = i.TranslatePoint(new Point(0, 0), this);
            return new Rect(topLeft.X, topLeft.Y, i.ActualWidth, i.ActualHeight);
        }

        Rect GetSelection(Point a, Point b)
        {
            b = new Point(b.X.Coerce(ActualWidth), b.Y.Coerce(ActualHeight));

            double
                x = a.X < b.X ? a.X : b.X,
                y = a.Y < b.Y ? a.Y : b.Y;

            //Now, the point is precisely what it should be
            var width
                = (a.X > b.X ? a.X - b.X : b.X - a.X).Absolute();
            var height
                = (a.Y > b.Y ? a.Y - b.Y : b.Y - a.Y).Absolute();

            return new Rect(new Point(x, y), new Size(width, height));
        }

        //...

        /// <summary>
        /// Scroll based on current position.
        /// </summary>
        /// <param name="point"></param>
        void Scroll(Point point)
        {
            double x = point.X, y = point.Y;

            //Up
            var y1 = scrollTolerance;
            var y1i = y1 - y;
            y1i = y1i < 0 ? y1i : 0;
            y1i = scrollOffset + y1i > scrollOffsetMaximum ? scrollOffsetMaximum : scrollOffset + y1i;

            //Bottom
            var y0 = Control.ActualHeight - scrollTolerance;
            var y0i = y - y0;
            y0i = y0i < 0 ? 0 : y0i;
            y0i = scrollOffset + y0i > scrollOffsetMaximum ? scrollOffsetMaximum : scrollOffset + y0i;

            //Right
            var x1 = Control.ActualWidth - scrollTolerance;
            var x1i = x - x1;
            x1i = x1i < 0 ? 0 : x1i;
            x1i = scrollOffset + x1i > scrollOffsetMaximum ? scrollOffsetMaximum : scrollOffset + x1i;

            //Left
            var x0 = scrollTolerance;
            var x0i = x0 - x;
            x0i = x0i < 0 ? 0 : x0i;
            x0i = scrollOffset + x0i > scrollOffsetMaximum ? scrollOffsetMaximum : scrollOffset + x0i;

            //Up
            if (y < y1)
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - y1i);

            //Bottom 
            else if (y > y0)
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + y0i);

            //Left  
            if (x < x0)
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - x0i);

            //Right
            else if (x > x1)
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + x1i);
        }

        //...

        /// <summary>
        /// Gets whether or not the given <see cref="Rect"/> intersects with any previous selection.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool? IntersectsWith(Rect input)
        {
            var j = 0;

            var result = false;
            Try.Invoke(() =>
            {
                foreach (var i in selections)
                {
                    if (input.IntersectsWith(i))
                    {
                        result = j % 2 == 0;
                        j++;
                    }
                }
            }, e => j = 0);
            return j == 0 ? null : (bool?)result;
        }

        //...

        /// <summary>
        /// Select items in control based on given area.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="area"></param>
        void Select(ItemsControl control, Rect area)
        {
            foreach (var i in control.Items)
            {
                var item = i is FrameworkElement j ? j : control.ItemContainerGenerator.ContainerFromItem(i) as FrameworkElement;
                if (item == null || item.Visibility != Visibility.Visible)
                    continue;

                var itemBounds = GetItemBounds(item);

                //Check if current (or previous) selection intersects with item bounds
                bool? intersectsWith = null;
                if (itemBounds.IntersectsWith(area))
                    intersectsWith = true;

                else if (itemBounds.IntersectsWith(previousSelection))
                    intersectsWith = false;

                bool? result = null;
                if ((ModifierKeys.Control.Pressed() || ModifierKeys.Shift.Pressed()))
                {
                    //Check whether or not the current item intersects with any previous selection
                    var intersectedWith = IntersectsWith(itemBounds);

                    //If current item has never insected with a previous selection...
                    if (intersectedWith == null)
                    {
                        result = intersectsWith;
                    }
                    else
                    {
                        result = intersectedWith.Value;
                        //If current item also intersects with current (or previous) selection, flip it once more
                        if (intersectsWith != null && intersectsWith.Value)
                            result = !result;
                    }
                }
                else result = intersectsWith;

                //If we are allowed to make a selection, make it
                if (result != null)
                    item.TrySelect(result.Value);

                //If TreeViewItem, repeat the above for each child
                if (item is TreeViewItem)
                    Select(item as ItemsControl, area);
            }
            previousSelection = area;
        }
    }
}