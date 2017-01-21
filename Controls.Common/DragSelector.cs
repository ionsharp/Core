using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Common.Primitives;
using Imagin.Controls.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// Provides logic for drag selecting over an ItemsControl.
    /// </summary>
    [TemplatePart(Name = "ScrollViewer", Type = typeof(ScrollViewer))]
    internal sealed class DragSelector : AbstractObject
    {
        #region Properties

        #region Parts

        /// <summary>
        /// Element defined in template that wraps <see cref="ItemsPresenter"/>; used for mouse events.
        /// </summary>
        Grid Grid { get; set; } = default(Grid);

        ItemsControl ItemsControl
        {
            get; set;
        }

        ScrollContentPresenter scrollContentPresenter = null;
        /// <summary>
        /// <see cref="System.Windows.Controls.ScrollContentPresenter"/> associated with <see cref="ScrollViewer"/>.
        /// </summary>
        ScrollContentPresenter ScrollContentPresenter
        {
            get
            {
                return scrollContentPresenter;
            }
            set
            {
                if (scrollContentPresenter != value)
                {
                    scrollContentPresenter = value;
                    if (value != null)
                        Hash = ScrollViewer.Style.GetHashCode();
                }
            }
        }

        /// <summary>
        /// Scrolling element defined in template.
        /// </summary>
        ScrollViewer ScrollViewer { get; set; } = default(ScrollViewer);

        #endregion

        #region Private

        /// <summary>
        /// Stores reference to ScrollViewer's style's hash code.
        /// </summary>
        int? Hash = null;

        /// <summary>
        /// Indicates if we're currently dragging.
        /// </summary>
        bool IsDragging { get; set; } = false;

        bool IsSingleMode
        {
            get
            {
                if (ItemsControl is ListBox)
                    return ItemsControl.As<ListBox>().SelectionMode == SelectionMode.Single;

                if (ItemsControl is DataGrid)
                    return ItemsControl.As<DataGrid>().SelectionMode == DataGridSelectionMode.Single;

                if (ItemsControl is TreeView)
                    return TreeViewExtensions.GetSelectionMode(ItemsControl.As<TreeView>()) == TreeViewSelectionMode.Single;

                return false;
            }
        }

        /// <summary>
        /// Stores reference to previously selected area.
        /// </summary>
        Rect PreviousSelection { get; set; } = new Rect();

        Selection selection;
        public Selection Selection
        {
            get
            {
                return selection;
            }
            set
            {
                selection = value;
                OnPropertyChanged("Selection");
            }
        }

        List<Rect> Selections { get; set; } = new List<Rect>();

        /// <summary>
        /// Point indicating where the drag started.
        /// </summary>
        Point StartPoint
        {
            get; set;
        }

        #endregion

        #endregion

        #region Methods

        #region Handlers

        void FindPresenter(object sender, EventArgs e)
        {
            FindPresenter();
        }

        /// <summary>
        /// Occurs when mouse is down; begins drag.
        /// </summary>
        async void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && !IsSingleMode)
                await OnDragStarted(e);
        }

        /// <summary>
        /// Ocurrs whenever mouse moves; drag is evaluated.
        /// </summary>
        async void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
                await OnDrag(e);
        }

        /// <summary>
        /// Occurs when mouse is up; ends drag.
        /// </summary>
        async void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && IsDragging)
                await OnDragEnded(e);
        }

        #endregion

        #region Private

        async Task<Rect> GetRect(Point StartPoint, Point CurrentPoint)
        {
            var Result = new Rect();

            double
                x = (StartPoint.X < CurrentPoint.X ? StartPoint.X : CurrentPoint.X),
                y = (StartPoint.Y < CurrentPoint.Y ? StartPoint.Y : CurrentPoint.Y);

            var Size = new Size(Selection.Width, Selection.Height);
            var Bounds = new Size(this.ScrollContentPresenter.Width, this.ScrollContentPresenter.Height);

            await Task.Run(new Action(() =>
            {
                Result = new Rect(new Point(x, y), Size).BoundPoint(Bounds);

                Result.Size = new Size(Math.Abs(CurrentPoint.X - StartPoint.X), Math.Abs(CurrentPoint.Y - StartPoint.Y));

                Result = Result.BoundSize(Bounds);
            }));

            return Result;
        }

        async Task OnDragStarted(MouseButtonEventArgs e)
        {
            Grid.CaptureMouse();

            StartPoint = e.GetPosition(Grid);

            IsDragging = true;

            if (!ModifierKeys.Control.IsPressed() && !ModifierKeys.Shift.IsPressed())
                Selections.TryClear();

            PreviousSelection = new Rect();
        }

        async Task OnDrag(MouseEventArgs e)
        {
            var Rect = await GetRect(StartPoint, e.GetPosition(Grid));
            if (IsDragging)
            {
                Selection.Set(Rect.X, Rect.Y, Rect.Width, Rect.Height);
                Select(ItemsControl, new Rect(ScrollContentPresenter.TranslatePoint(Selection.TopLeft, ScrollContentPresenter), ScrollContentPresenter.TranslatePoint(Selection.BottomRight, ScrollContentPresenter)));
                Scroll(e.GetPosition(ItemsControl));
            }
        }

        async Task OnDragEnded(MouseButtonEventArgs e)
        {
            IsDragging = false;

            if (Grid.IsMouseCaptured)
                Grid.ReleaseMouseCapture();

            if (!Selections.TryAdd(PreviousSelection))
                Selections.TryClear();

            Selection.Set(0d, 0d, 0d, 0d);

            StartPoint = default(Point);
        }

        /// <summary>
        /// Find and store reference to <see cref="ScrollContentPresenter"/> by searching <see cref="ScrollViewer"/> template.
        /// </summary>
        void FindPresenter()
        {
            if (ScrollViewer != null && ScrollViewer.Style != null && (Hash == null || ScrollViewer.Style.GetHashCode() != Hash.Value))
            {
                foreach (var e in ScrollViewer.GetVisualChildren<FrameworkElement>())
                {
                    if (e.Is<ScrollContentPresenter>())
                    {
                        ScrollContentPresenter = e as ScrollContentPresenter;
                        break;
                    }
                }
            }
        }

        Rect GetItemBounds(FrameworkElement Item)
        {
            var TopLeft = Item.TranslatePoint(new Point(0, 0), Grid);
            return new Rect(TopLeft.X, TopLeft.Y, Item.ActualWidth, Item.ActualHeight);
        }

        /// <summary>
        /// Scroll based on current position.
        /// </summary>
        /// <param name="Position"></param>
        void Scroll(Point Position)
        {
            double HorizontalPosition = Position.X, VerticalPosition = Position.Y;

            var ScrollOffset = ItemsControlExtensions.GetDragScrollOffset(ItemsControl);
            var ScrollTolerance = ItemsControlExtensions.GetDragScrollTolerance(ItemsControl);

            //Cursor is at top, scroll up.
            if (VerticalPosition < ScrollTolerance)
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - ScrollOffset);
            //Cursor is at bottom, scroll down.  
            else if (VerticalPosition > ItemsControl.ActualHeight - ScrollTolerance) //Bottom of visible list?
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + ScrollOffset);
            //Cursor is at left, scroll left.  
            else if (HorizontalPosition < ScrollTolerance)
                ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - ScrollOffset);
            //Cursor is at right, scroll right.  
            else if (HorizontalPosition > ItemsControl.ActualWidth - ScrollTolerance)
                ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + ScrollOffset);
        }

        /// <summary>
        /// Gets whether or not the given <see cref="Rect"/> intersects with either of the other two given <see cref="Rect"/>s: True if first, false if second, null if neither.
        /// </summary>
        /// <param name="Rect1"></param>
        /// <param name="Rect2"></param>
        /// <param name="Rect3"></param>
        /// <returns></returns>
        bool? IntersectsWith(Rect Rect1, Rect Rect2, Rect Rect3)
        {
            if (Rect1.IntersectsWith(Rect2))
            {
                return true;
            }
            else if (Rect1.IntersectsWith(Rect3))
            {
                return false;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets whether or not the given <see cref="Rect"/> intersects with any previous selection.
        /// </summary>
        /// <param name="Bounds"></param>
        /// <returns></returns>
        bool? IntersectedWith(Rect Bounds)
        {
            var u = 0;
            var v = false;

            Selections.TryForEach<Rect>(y =>
            {
                if (Bounds.IntersectsWith(y))
                {
                    v = u % 2 == 0;
                    u++;
                }
            });

            return u == 0 ? null : (bool?)v;
        }

        /// <summary>
        /// Select items in control based on given area.
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Area"></param>
        void Select(ItemsControl Control, Rect Area)
        {
            foreach (var i in Control.Items)
            {
                //Get visual object from data object
                var Item = i is FrameworkElement ? i as FrameworkElement : Control.ItemContainerGenerator.ContainerFromItem(i) as FrameworkElement;

                if (Item == null || Item.Visibility != Visibility.Visible)
                    continue;

                //Get item bounds
                var ItemBounds = GetItemBounds(Item);

                //Check if current (or previous) selection intersects with item bounds
                var intersectsWith = IntersectsWith(ItemBounds, Area, PreviousSelection);

                var Result = default(bool?);
                if ((ModifierKeys.Control.IsPressed() || ModifierKeys.Shift.IsPressed()))
                {
                    //Check whether or not the current item intersects with any previous selection
                    var intersectedWith = IntersectedWith(ItemBounds);

                    //If current item has never insected with a previous selection...
                    if (intersectedWith == null)
                    {
                        Result = intersectsWith;
                    }
                    else
                    {
                        Result = intersectedWith.Value;

                        //If current item also intersects with current (or previous) selection, flip it once more
                        if (intersectsWith != null && intersectsWith.Value)
                            Result = !Result;
                    }
                }
                else Result = intersectsWith;

                //If we are allowed to make a selection, make it
                if (Result != null)
                    Item.TrySelect(Result.Value);

                //If visible TreeViewItem, enumerate its children
                if (Item is TreeViewItem)
                    Select(Item as ItemsControl, Area);
            }
            PreviousSelection = Area;
        }

        #endregion

        #region Public

        public static DragSelector New(ItemsControl ItemsControl)
        {
            return new DragSelector(ItemsControl);
        }

        public void Register()
        {
            Grid.MouseDown += OnMouseDown;
            Grid.MouseMove += OnMouseMove;
            Grid.MouseUp += OnMouseUp;

            ScrollViewer.LayoutUpdated += FindPresenter;
            ScrollViewer.Loaded += FindPresenter;
        }

        public void Deregister()
        {
            Grid.MouseDown -= OnMouseDown;
            Grid.MouseMove -= OnMouseMove;
            Grid.MouseUp -= OnMouseUp;

            ScrollViewer.LayoutUpdated -= FindPresenter;
            ScrollViewer.Loaded -= FindPresenter;
        }

        #endregion

        #endregion

        #region DragSelector

        DragSelector(ItemsControl itemsControl) : base()
        {
            ItemsControl = itemsControl;

            ScrollViewer = ItemsControl.Template.FindName("ScrollViewer", ItemsControl) as ScrollViewer;
            if (ScrollViewer == null)
                throw new KeyNotFoundException("ScrollViewer cannot be null.");

            if (ScrollViewer.Content.IsNot<Grid>())
                throw new KeyNotFoundException("ItemsPresenter must be wrapped.");

            Grid = ScrollViewer.Content as Grid;
            Grid.Background = Brushes.Transparent; //Required for mouse events

            Selection = new Rect(0d, 0d, 0d, 0d);
            
            var Temp = new DragSelection();
            Temp.Bind(DragSelection.SelectionProperty, this, "Selection");
            Grid.Children.Add(Temp);
        }

        #endregion
    }
}
