using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Common.Primitives;
using Imagin.Controls.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using System.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// Provides logic for drag selecting over an ItemsControl.
    /// </summary>
    [TemplatePart(Name = "PART_Grid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    internal sealed class DragSelector : AbstractObject
    {
        #region Properties

        ItemsControl ItemsControl
        {
            get; set;
        }

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

        #region Parts

        DragSelection PART_DragSelection
        {
            get; set;
        }

        Grid PART_Grid
        {
            get; set;
        }

        ScrollContentPresenter ScrollContentPresenter
        {
            get; set;
        }

        ScrollViewer PART_ScrollViewer
        {
            get; set;
        }

        #endregion

        #region Private

        /// <summary>
        /// Stores reference to ScrollViewer's style's hash code.
        /// </summary>
        int? Hash = null;

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
        /// Indicates if we're currently dragging.
        /// </summary>
        bool IsDragging { get; set; } = false;

        List<Rect> Selections { get; set; } = new List<Rect>();

        /// <summary>
        /// Stores reference to previously selected area.
        /// </summary>
        Rect PreviousSelection { get; set; } = new Rect();

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
            PART_Grid.CaptureMouse();

            StartPoint = e.GetPosition(PART_Grid);

            IsDragging = true;

            if (!ModifierKeys.Control.IsPressed() && !ModifierKeys.Shift.IsPressed())
                Selections.TryClear();

            PreviousSelection = new Rect();
        }

        async Task OnDrag(MouseEventArgs e)
        {
            var Rect = await GetRect(StartPoint, e.GetPosition(PART_Grid));
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

            if (PART_Grid.IsMouseCaptured)
                PART_Grid.ReleaseMouseCapture();

            if (!Selections.TryAdd(PreviousSelection))
                Selections.TryClear();

            Selection.Set(0d, 0d, 0d, 0d);

            StartPoint = default(Point);
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
                PART_ScrollViewer.ScrollToVerticalOffset(PART_ScrollViewer.VerticalOffset - ScrollOffset);
            //Cursor is at bottom, scroll down.  
            else if (VerticalPosition > ItemsControl.ActualHeight - ScrollTolerance) //Bottom of visible list?
                PART_ScrollViewer.ScrollToVerticalOffset(PART_ScrollViewer.VerticalOffset + ScrollOffset);
            //Cursor is at left, scroll left.  
            else if (HorizontalPosition < ScrollTolerance)
                PART_ScrollViewer.ScrollToHorizontalOffset(PART_ScrollViewer.HorizontalOffset - ScrollOffset);
            //Cursor is at right, scroll right.  
            else if (HorizontalPosition > ItemsControl.ActualWidth - ScrollTolerance)
                PART_ScrollViewer.ScrollToHorizontalOffset(PART_ScrollViewer.HorizontalOffset + ScrollOffset);
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
                var Item = Control.ItemContainerGenerator.ContainerFromItem(i) as FrameworkElement;

                if (Item == null || Item.Visibility.EqualsAny(Visibility.Collapsed))
                    continue;

                //Evaluate item
                var TopLeft = Item.TranslatePoint(new Point(0, 0), PART_Grid);
                var ItemBounds = new Rect(TopLeft.X, TopLeft.Y, Item.ActualWidth, Item.ActualHeight);

                var Hierarchial = Control is TreeView || Control is TreeViewItem;

                var Result = default(bool?);

                if ((ModifierKeys.Control.IsPressed() || ModifierKeys.Shift.IsPressed()))
                {
                    if (Hierarchial)
                    {
                        Select(Item as TreeViewItem, true, Area);
                    }
                    else
                    {
                        //Get whether or not the current item intersects with any previous selection
                        var intersectedWith = IntersectedWith(ItemBounds);

                        //If current item has never insected with a previous selection...
                        if (intersectedWith == null)
                        {
                            Result = IntersectsWith(ItemBounds, Area, PreviousSelection);
                        }
                        else
                        {
                            Result = intersectedWith.Value;

                            var w = IntersectsWith(ItemBounds, Area, PreviousSelection);

                            //If current item also intersects with current selection, flip it once more
                            if (w != null && w.Value)
                                Result = !Result;
                        }
                    }
                }
                else
                {
                    if (Hierarchial)
                    {
                        Select(Item as TreeViewItem, true, Area);
                    }
                    else Result = IntersectsWith(ItemBounds, Area, PreviousSelection);
                }

                if (Result != null)
                    Item.TrySelect(Result.Value);
            }
            PreviousSelection = Area;
        }

        /// <summary>
        /// Select items in <see cref="TreeViewItem"/> based on given area.
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="IsSelected"></param>
        /// <param name="Area"></param>
        void Select(TreeViewItem Item, bool IsSelected, Rect Area)
        {
            TreeViewItemExtensions.SetIsSelected(Item, IsSelected);
            if (Item.IsExpanded && Item.Items.Count > 0)
                Select(Item, Area);
        }

        /// <summary>
        /// Find and store reference to <see cref="ScrollContentPresenter"/> by searching <see cref="ScrollViewer"/> template.
        /// </summary>
        void SetPresenter()
        {
            if (PART_ScrollViewer != null && PART_ScrollViewer.Style != null && (Hash == null || PART_ScrollViewer.Style.GetHashCode() != Hash.Value))
            {
                foreach (var Element in PART_ScrollViewer.GetVisualChildren<FrameworkElement>())
                {
                    if (Element.Is<ScrollContentPresenter>())
                    {
                        ScrollContentPresenter = Element as ScrollContentPresenter;
                        Hash = PART_ScrollViewer.Style.GetHashCode();
                        break;
                    }
                }
            }
        }

        #endregion

        #region Public

        public static DragSelector New(ItemsControl ItemsControl)
        {
            return new DragSelector(ItemsControl);
        }

        public void Register()
        {
            PART_Grid.MouseDown += this.OnMouseDown;
            PART_Grid.MouseMove += this.OnMouseMove;
            PART_Grid.MouseUp += this.OnMouseUp;

            PART_ScrollViewer.LayoutUpdated += SetPresenter;
            PART_ScrollViewer.Loaded += SetPresenter;
        }

        public void Deregister()
        {
            PART_Grid.MouseDown -= this.OnMouseDown;
            PART_Grid.MouseMove -= this.OnMouseMove;
            PART_Grid.MouseUp -= this.OnMouseUp;

            PART_ScrollViewer.LayoutUpdated -= SetPresenter;
            PART_ScrollViewer.Loaded -= SetPresenter;
        }

        #endregion

        #region Events

        void SetPresenter(object sender, EventArgs e)
        {
            this.SetPresenter();
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

        #endregion

        #region DragSelector

        DragSelector(ItemsControl itemsControl) : base()
        {
            ItemsControl = itemsControl;

            PART_Grid = ItemsControl.Template.FindName("PART_Grid", ItemsControl) as Grid;
            if (PART_Grid == null)
                throw new KeyNotFoundException("Grid cannot be null.");

            //Required for mouse events
            PART_Grid.Background = Brushes.Transparent;

            PART_ScrollViewer = ItemsControl.Template.FindName("PART_ScrollViewer", ItemsControl) as ScrollViewer;
            if (PART_ScrollViewer == null)
                throw new KeyNotFoundException("ScrollViewer cannot be null.");

            PART_ScrollViewer.Background = Brushes.Transparent;

            Selection = new Rect(0d, 0d, 0d, 0d);

            //Create and add selection control to grid
            PART_DragSelection = new DragSelection();
            BindingOperations.SetBinding(PART_DragSelection, DragSelection.SelectionProperty, new Binding()
            {
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath("Selection"),
                Source = this
            });

            PART_Grid.Children.Add(PART_DragSelection);
        }

        #endregion
    }
}
