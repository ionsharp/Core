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

namespace Imagin.Controls.Common
{
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

        #region Private

        /// <summary>
        /// Stores reference to previously selected area.
        /// </summary>
        Rect PreviousArea;

        /// <summary>
        /// Indicates if we're currently dragging.
        /// </summary>
        bool IsDragging = false;

        /// <summary>
        /// Point indicating where the drag started.
        /// </summary>
        Point StartDrag;

        /// <summary>
        /// Stores reference to ScrollViewer's style's hash code.
        /// </summary>
        int? Hash = null;

        #endregion

        #region References

        DragSelection DragSelection
        {
            get; set;
        }

        ScrollContentPresenter ScrollContent
        {
            get; set;
        }

        ScrollViewer ScrollViewer
        {
            get; set;
        }

        Grid Grid
        {
            get; set;
        }

        #endregion

        #endregion

        #region Methods

        #region Private

        /// <summary>
        /// Ensures given point values do not exceed canvas bounds.
        /// </summary>
        async Task<Point> BoundPoint(double x, double y)
        {
            return await this.BoundPoint(x, y, new Size(this.ItemsControl.Width, this.ItemsControl.Height), new Size(this.IDragSelector.Selection.Width, this.IDragSelector.Selection.Height));
        }

        /// <summary>
        /// Ensures given point values do not exceed canvas bounds.
        /// </summary>
        async Task<Point> BoundPoint(double x, double y, Size ItemsControlSize, Size SelectionSize)
        {
            Point Result = default(Point);
            await Task.Run(new Action(() =>
            {
                x = x < 0 ? 0 : x;
                y = y < 0 ? 0 : y;
                var NewX = ItemsControlSize.Width - SelectionSize.Width;
                var NewY = ItemsControlSize.Height - SelectionSize.Height;
                x = x > NewX ? NewX : x;
                y = y > NewY ? NewY : y;
                Result = new Point(x, y);
            }));
            return Result;
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
            Size Result = default(Size);
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
        /// Finds a child element by type in given element.
        /// </summary>
        static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Find ScrollContentPresenter element in ScrollViewer template.
        /// </summary>
        void FindScrollContent()
        {
            if (this.ScrollViewer == null)
                return;
            foreach (FrameworkElement Element in DragSelector.FindVisualChildren<FrameworkElement>(this.ScrollViewer))
            {
                if (!Element.Is<ScrollContentPresenter>())
                    continue;
                this.ScrollContent = Element as ScrollContentPresenter;
                this.Hash = this.ScrollViewer.Style.GetHashCode();
                Console.WriteLine(this.Hash.ToString());
                break;
            }
        }

        /// <summary>
        /// Checks if either control or shift key is pressed.
        /// </summary>
        bool IsModifierPressed()
        {
            return (Keyboard.Modifiers & ModifierKeys.Control) != 0 || (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
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

        /// <summary>
        /// Updates selection when drag selection is enabled and active.
        /// </summary>
        void UpdateSelected(Rect Area)
        {
            //Check if selection should be appended are replaced. We allow appending when either control or shift key is pressed.
            if (!this.IsModifierPressed())
                this.TryClearSelected();
            for (int i = 0; i < this.ItemsControl.Items.Count; i++)
            {
                //Get reference to visual object from data object
                FrameworkElement Item = this.ItemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                if (Item == null)
                    continue;

                //Get item bounds based on parent.
                Point TopLeft = Item.TranslatePoint(new Point(0, 0), this.Grid);
                Rect ItemBounds = new Rect(TopLeft.X, TopLeft.Y, Item.ActualWidth, Item.ActualHeight);

                // Only change the selection if it intersects (or previously intersected) with the given area.
                bool? IsSelected = null;
                if (ItemBounds.IntersectsWith(Area))
                    IsSelected = true;
                else if (ItemBounds.IntersectsWith(this.PreviousArea))
                    IsSelected = false;
                if (IsSelected != null)
                    this.TrySelect(Item, IsSelected.Value);
            }
            this.PreviousArea = Area;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when mouse is down; begins drag.
        /// </summary>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.IDragSelector.IsDragSelectionEnabled || e.ChangedButton != MouseButton.Left)
                return;
            this.Grid.CaptureMouse();
            this.IsDragging = true;
            this.PreviousArea = new Rect();
            this.StartDrag = e.GetPosition(this.Grid);
            this.IDragSelector.Selection.Set(this.StartDrag.X, this.StartDrag.Y, 0, 0);
        }

        /// <summary>
        /// Ocurrs whenever mouse moves; drag is evaluated.
        /// </summary>
        async void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.IsDragging)
                return;

            Point CurrentPosition = e.GetPosition(this.Grid);

            //Calculate new position
            double
                x = (this.StartDrag.X < CurrentPosition.X ? this.StartDrag.X : CurrentPosition.X),
                y = (this.StartDrag.Y < CurrentPosition.Y ? this.StartDrag.Y : CurrentPosition.Y);

            Size NewSize = await this.GetSize(CurrentPosition, this.StartDrag);
            Point NewPosition = await this.BoundPoint(x, y);
            NewSize = await this.BoundSize(NewSize, NewPosition, new Size(this.ScrollContent.Width, this.ScrollContent.Height));

            //Check again since this method runs asynchronously
            if (e.LeftButton != MouseButtonState.Pressed || !this.Grid.IsMouseCaptured)
                return;

            //Update visual selection
            this.IDragSelector.Selection.Set(NewPosition.X, NewPosition.Y, NewSize.Width, NewSize.Height);

            //Update selected items
            this.UpdateSelected(new Rect(this.ScrollContent.TranslatePoint(this.IDragSelector.Selection.TopLeft, this.ItemsControl), this.ScrollContent.TranslatePoint(this.IDragSelector.Selection.BottomRight, this.ItemsControl)));

            double HorizontalPosition = e.GetPosition(this.ItemsControl).X;
            double VerticalPosition = e.GetPosition(this.ItemsControl).Y;
            //Cursor is at top, scroll up.
            if (VerticalPosition < this.IDragSelector.ScrollTolerance)
                this.ScrollViewer.ScrollToVerticalOffset(this.ScrollViewer.VerticalOffset - this.IDragSelector.ScrollOffset);
            //Cursor is at bottom, scroll down.  
            else if (VerticalPosition > this.ItemsControl.ActualHeight - this.IDragSelector.ScrollTolerance) //Bottom of visible list?
                this.ScrollViewer.ScrollToVerticalOffset(this.ScrollViewer.VerticalOffset + this.IDragSelector.ScrollOffset);
            //Cursor is at left, scroll left.  
            else if (HorizontalPosition < this.IDragSelector.ScrollTolerance)
                this.ScrollViewer.ScrollToHorizontalOffset(this.ScrollViewer.HorizontalOffset - this.IDragSelector.ScrollOffset);
            //Cursor is at right, scroll right.  
            else if (HorizontalPosition > this.ItemsControl.ActualWidth - this.IDragSelector.ScrollTolerance)
                this.ScrollViewer.ScrollToHorizontalOffset(this.ScrollViewer.HorizontalOffset + this.IDragSelector.ScrollOffset);
        }

        /// <summary>
        /// Occurs when mouse is up; ends drag.
        /// </summary>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.IDragSelector.IsDragSelectionEnabled || e.LeftButton != MouseButtonState.Released || !this.Grid.IsMouseCaptured)
                return;
            this.Grid.ReleaseMouseCapture();
            this.IsDragging = false;
            this.IDragSelector.Selection.Width = 0;
            this.IDragSelector.Selection.Height = 0;
            //Check if mouse has moved at all since pressing left button. If not, clear selection. Blank space was clicked.
            Point CurrentPosition = e.GetPosition(this.Grid);
            if (this.StartDrag == CurrentPosition)
                this.TryClearSelected();
        }

        /// <summary>
        /// Occurs when ScrollViewer's layout has updated.
        /// </summary>
        void OnScrollViewerLayoutUpdated(object sender, EventArgs e)
        {
            if (this.Hash == null || this.ScrollViewer.Style.GetHashCode() != this.Hash.Value)
                this.FindScrollContent();
        }

        /// <summary>
        /// Occurs when ScrollViewer has loaded; gets initial reference to ScrollContentPresenter.
        /// </summary>
        void OnScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            this.FindScrollContent();
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
            this.IDragSelector.Selection = new Selection(0.0, 0.0, 0.0, 0.0);

            this.Grid = this.ItemsControl.Template.FindName("PART_Grid", this.ItemsControl) as Grid;
            if (this.Grid == null)
                throw new KeyNotFoundException("Grid cannot be null.");
            this.Grid.MouseDown += this.OnMouseDown;
            this.Grid.MouseMove += this.OnMouseMove;
            this.Grid.MouseUp += this.OnMouseUp;

            this.ScrollViewer = this.ItemsControl.Template.FindName("PART_ScrollViewer", this.ItemsControl) as ScrollViewer;
            if (this.ScrollViewer == null)
                throw new KeyNotFoundException("ScrollViewer cannot be null.");
            this.ScrollViewer.LayoutUpdated += OnScrollViewerLayoutUpdated;
            this.ScrollViewer.Loaded += OnScrollViewerLoaded;

            this.DragSelection = this.ItemsControl.Template.FindName("PART_DragSelection", this.ItemsControl) as DragSelection;
            if (this.DragSelection == null)
                throw new KeyNotFoundException("DragSelection cannot be null.");
        }

        #endregion
    }
}
