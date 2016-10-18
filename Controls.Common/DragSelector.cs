using Imagin.Common;
using Imagin.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Imagin.Common.Primitives;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_DragSelection", Type = typeof(DragSelection))]
    [TemplatePart(Name = "PART_Grid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    public sealed class DragSelector : AbstractObject
    {
        #region Properties

        ItemsControl ItemsControl
        {
            get; set;
        }

        IDragSelector IDragSelector
        {
            get; set;
        }

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

        /// <summary>
        /// Stores reference to ScrollViewer's style's hash code.
        /// </summary>
        int? Hash = null;

        bool HasSingleSelectionMode
        {
            get
            {
                if (this.ItemsControl is ListBox)
                    return this.ItemsControl.As<ListBox>().SelectionMode == SelectionMode.Single;
                if (this.ItemsControl is DataGrid)
                    return this.ItemsControl.As<DataGrid>().SelectionMode == DataGridSelectionMode.Single;
                if (this.ItemsControl is AdvancedTreeView)
                    return this.ItemsControl.As<AdvancedTreeView>().SelectionMode == DataGridSelectionMode.Single;
                return false;
            }
        }

        /// <summary>
        /// Indicates if we're currently dragging.
        /// </summary>
        bool IsDragging = false;

        /// <summary>
        /// Stores reference to previously selected area.
        /// </summary>
        Rect PreviousArea;

        /// <summary>
        /// Point indicating where the drag started.
        /// </summary>
        Point StartDrag;

        #endregion

        #region Methods

        #region Private

        void AdjustScroll(Point Position)
        {
            double HorizontalPosition = Position.X, VerticalPosition = Position.Y;
            //Cursor is at top, scroll up.
            if (VerticalPosition < this.IDragSelector.DragScrollTolerance)
                this.PART_ScrollViewer.ScrollToVerticalOffset(this.PART_ScrollViewer.VerticalOffset - this.IDragSelector.DragScrollOffset);
            //Cursor is at bottom, scroll down.  
            else if (VerticalPosition > this.ItemsControl.ActualHeight - this.IDragSelector.DragScrollTolerance) //Bottom of visible list?
                this.PART_ScrollViewer.ScrollToVerticalOffset(this.PART_ScrollViewer.VerticalOffset + this.IDragSelector.DragScrollOffset);
            //Cursor is at left, scroll left.  
            else if (HorizontalPosition < this.IDragSelector.DragScrollTolerance)
                this.PART_ScrollViewer.ScrollToHorizontalOffset(this.PART_ScrollViewer.HorizontalOffset - this.IDragSelector.DragScrollOffset);
            //Cursor is at right, scroll right.  
            else if (HorizontalPosition > this.ItemsControl.ActualWidth - this.IDragSelector.DragScrollTolerance)
                this.PART_ScrollViewer.ScrollToHorizontalOffset(this.PART_ScrollViewer.HorizontalOffset + this.IDragSelector.DragScrollOffset);
        }

        /// <summary>
        /// Ensures given point values do not exceed canvas bounds.
        /// </summary>
        async Task<Point> BoundPoint(double x, double y)
        {
            return await this.BoundPoint(x, y, new Size(this.ScrollContentPresenter.Width, this.ScrollContentPresenter.Height), new Size(this.IDragSelector.DragSelection.Width, this.IDragSelector.DragSelection.Height));
        }

        /// <summary>
        /// Ensures given point values do not exceed canvas bounds.
        /// </summary>
        async Task<Point> BoundPoint(double x, double y, Size ScrollContentSize, Size SelectionSize)
        {
            Point Result = default(Point);
            await Task.Run(new Action(() =>
            {
                x = x < 0 ? 0 : x;
                y = y < 0 ? 0 : y;

                var xa = ScrollContentSize.Width - SelectionSize.Width;
                var ya = ScrollContentSize.Height - SelectionSize.Height;

                x = x > xa ? xa : x;
                y = y > ya ? ya : y;
                Result = new Point(x, y);
            }));
            return Result;
        }

        async Task<PointSize> GetPointSize(Point CurrentPosition)
        {
            //Calculate new position
            double
                x = (this.StartDrag.X < CurrentPosition.X ? this.StartDrag.X : CurrentPosition.X),
                y = (this.StartDrag.Y < CurrentPosition.Y ? this.StartDrag.Y : CurrentPosition.Y);

            var NewSize = await this.GetSize(CurrentPosition, this.StartDrag);
            var NewPosition = await this.BoundPoint(x, y);
            NewSize = await this.BoundSize(NewSize, NewPosition, new Size(this.ScrollContentPresenter.Width, this.ScrollContentPresenter.Height));

            return new PointSize(NewPosition, NewSize);
        }

        /// <summary>
        /// Calculate new size
        /// </summary>
        async Task<Size> GetSize(Point CurrentPosition, Point StartDrag)
        {
            Size Result = default(Size);
            await Task.Run(new Action(() => Result = new Size(Math.Abs(CurrentPosition.X - StartDrag.X), Math.Abs(CurrentPosition.Y - StartDrag.Y))));
            return Result;
        }

        /// <summary>
        /// Ensures given size does not exceed canvas bounds.
        /// </summary>
        async Task<Size> BoundSize(Size Size, Point Point, Size ScrollContentSize)
        {
            var Result = default(Size);
            await Task.Run(new Action(() =>
            {
                double Width =
                    Size.Width + Point.X > ScrollContentSize.Width
                        ? ScrollContentSize.Width - Point.X
                        : Size.Width;
                double Height =
                    Size.Height + Point.Y > ScrollContentSize.Height
                        ? ScrollContentSize.Height - Point.Y
                        : Size.Height;
                Result = new Size(Width < 0.0 ? 0.0 : Width, Height < 0.0 ? 0.0 : Height);
            }));
            return Result;
        }

        async Task<bool> BeginIntersectsWith(Rect Rect1, Rect Rect2)
        {
            bool Result = false;
            await Task.Run(new Action(() => Result = Rect1.IntersectsWith(Rect2)));
            return Result;
        }

        /// <summary>
        /// Checks if either control or shift key is pressed.
        /// </summary>
        bool IsModifierPressed()
        {
            return (Keyboard.Modifiers & ModifierKeys.Control) != 0 || (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
        }

        /// <summary>
        /// Find ScrollContentPresenter element in ScrollViewer template.
        /// </summary>
        void SetScrollContentPresenter()
        {
            if (this.PART_ScrollViewer != null && this.PART_ScrollViewer.Style != null && (this.Hash == null || this.PART_ScrollViewer.Style.GetHashCode() != this.Hash.Value))
            {
                foreach (var Element in this.PART_ScrollViewer.GetVisualChildren<FrameworkElement>())
                {
                    if (!Element.Is<ScrollContentPresenter>())
                        continue;
                    this.ScrollContentPresenter = Element as ScrollContentPresenter;
                    this.Hash = this.PART_ScrollViewer.Style.GetHashCode();
                    //Console.WriteLine(this.Hash.ToString());
                    break;
                }
            }
        }

        /// <summary>
        /// Updates selection when drag selection is enabled and active.
        /// </summary>
        void SetSelected(ItemsControl Control, Rect Area)
        {
            //Check if selection should be appended are replaced. We allow appending when either control or shift key is pressed.
            if (!this.IsModifierPressed())
                this.TryClearSelected();

            foreach (var i in Control.Items)
            {
                //Get reference to visual object from data object
                var Item = Control.ItemContainerGenerator.ContainerFromItem(i) as FrameworkElement;
                if (Item == null || Item.Visibility.EqualsAny(Visibility.Collapsed)) continue;

                //Evaluate item
                var TopLeft = Item.TranslatePoint(new Point(0, 0), this.PART_Grid);
                var ItemBounds = new Rect(TopLeft.X, TopLeft.Y, Item.ActualWidth, Item.ActualHeight);

                bool? IsSelected = null;
                if (ItemBounds.IntersectsWith(Area))
                    IsSelected = true;
                else if (ItemBounds.IntersectsWith(this.PreviousArea))
                    IsSelected = false;

                if (Control.IsAny(typeof(AdvancedTreeView), typeof(TreeViewItem)))
                {
                    var TreeViewItem = Item as TreeViewItem;
                    if (IsSelected != null)
                        AdvancedTreeView.SetIsItemSelected(TreeViewItem, IsSelected.Value);
                    //Evaluate children
                    if (TreeViewItem.IsExpanded && TreeViewItem.Items.Count > 0)
                        this.SetSelected(TreeViewItem, Area);
                }
                else
                {
                    if (IsSelected != null)
                        this.TrySelect(Item, IsSelected.Value);
                }
            }
            this.PreviousArea = Area;
        }

        /// <summary>
        /// Attempts to clear the current selection.
        /// </summary>
        bool TryClearSelected()
        {
            try
            {
                if (this.ItemsControl is ListBox)
                    (this.ItemsControl as ListBox).SelectedItems.Clear();
                else if (this.ItemsControl is DataGrid)
                    (this.ItemsControl as DataGrid).SelectedItems.Clear();
                else if (this.ItemsControl is AdvancedTreeView)
                    this.ItemsControl.As<AdvancedTreeView>().SelectNone();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to select or deselect a ListViewItem.
        /// </summary>
        bool TrySelect(DependencyObject Item, bool IsSelected)
        {
            try
            {
                Selector.SetIsSelected(Item, IsSelected);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when mouse is down; begins drag.
        /// </summary>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || !this.IDragSelector.IsDragSelectionEnabled || this.HasSingleSelectionMode) return;
            this.PART_Grid.CaptureMouse();

            this.IsDragging = true;

            this.PreviousArea = new Rect();
            this.StartDrag = e.GetPosition(this.PART_Grid);

            this.IDragSelector.DragSelection.Set(this.StartDrag.X, this.StartDrag.Y, 0, 0);
        }

        /// <summary>
        /// Ocurrs whenever mouse moves; drag is evaluated.
        /// </summary>
        async void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.IsDragging) return;

            var PointSize = await this.GetPointSize(e.GetPosition(this.PART_Grid));

            //Check again since this method runs asynchronously
            if (!this.IsDragging) return;

            //Update visual selection
            this.IDragSelector.DragSelection.Set(PointSize.Point.X, PointSize.Point.Y, PointSize.Size.Width, PointSize.Size.Height);

            //Update selected items
            this.SetSelected(this.ItemsControl, new Rect(this.ScrollContentPresenter.TranslatePoint(this.IDragSelector.DragSelection.TopLeft, this.ScrollContentPresenter), this.ScrollContentPresenter.TranslatePoint(this.IDragSelector.DragSelection.BottomRight, this.ScrollContentPresenter)));

            this.AdjustScroll(e.GetPosition(this.ItemsControl));
        }

        /// <summary>
        /// Occurs when mouse is up; ends drag.
        /// </summary>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.IDragSelector.IsDragSelectionEnabled || e.LeftButton != MouseButtonState.Released || !this.PART_Grid.IsMouseCaptured) return;
            this.PART_Grid.ReleaseMouseCapture();

            this.IsDragging = false;
            this.IDragSelector.DragSelection.Width = 0;
            this.IDragSelector.DragSelection.Height = 0;

            //Check if mouse has moved since pressing left button. If not, blank space was clicked, clear selection.
            var CurrentPosition = e.GetPosition(this.PART_Grid);
            if (this.StartDrag == CurrentPosition)
                this.TryClearSelected();
        }

        #endregion

        #endregion

        #region DragSelector

        internal DragSelector(ItemsControl ItemsControl)
        {
            this.ItemsControl = ItemsControl;
            if (this.ItemsControl == null || !(this.ItemsControl is IDragSelector))
                throw new InvalidCastException("ItemsControl is either null or not of type IDragSelector.");

            this.IDragSelector = this.ItemsControl as IDragSelector;
            this.IDragSelector.DragSelection = new Selection(0.0, 0.0, 0.0, 0.0);

            this.PART_Grid = this.ItemsControl.Template.FindName("PART_Grid", this.ItemsControl) as Grid;
            if (this.PART_Grid == null)
                throw new KeyNotFoundException("Grid cannot be null.");
            this.PART_Grid.MouseDown += this.OnMouseDown;
            this.PART_Grid.MouseMove += this.OnMouseMove;
            this.PART_Grid.MouseUp += this.OnMouseUp;

            this.PART_ScrollViewer = this.ItemsControl.Template.FindName("PART_ScrollViewer", this.ItemsControl) as ScrollViewer;
            if (this.PART_ScrollViewer == null)
                throw new KeyNotFoundException("ScrollViewer cannot be null.");
            this.PART_ScrollViewer.LayoutUpdated += (s, e) => this.SetScrollContentPresenter();
            this.PART_ScrollViewer.Loaded += (s, e) => this.SetScrollContentPresenter();

            this.PART_DragSelection = this.ItemsControl.Template.FindName("PART_DragSelection", this.ItemsControl) as DragSelection;
            if (this.PART_DragSelection == null)
                throw new KeyNotFoundException("DragSelection cannot be null.");
        }

        #endregion
    }
}
