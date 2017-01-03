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

namespace Imagin.Controls.Common
{
    /// <summary>
    /// Provides logic for enabling drag selection on an ItemsControl.
    /// </summary>
    [TemplatePart(Name = "PART_Grid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    internal sealed class DragSelectionManager : AbstractObject
    {
        #region Properties

        ItemsControl ItemsControl
        {
            get; set;
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

        bool HasSingleSelectionMode
        {
            get
            {
                if (this.ItemsControl is ListBox)
                    return this.ItemsControl.As<ListBox>().SelectionMode == SelectionMode.Single;
                if (this.ItemsControl is DataGrid)
                    return this.ItemsControl.As<DataGrid>().SelectionMode == DataGridSelectionMode.Single;
                if (this.ItemsControl is TreeView)
                    return TreeViewExtensions.GetSelectionMode(this.ItemsControl.As<TreeView>()) == TreeViewSelectionMode.Single;
                return false;
            }
        }

        /// <summary>
        /// Indicates if we're currently dragging.
        /// </summary>
        bool IsDragging = false;

        bool IsModifierKeyPressed
        {
            get
            {
                return (Keyboard.Modifiers & ModifierKeys.Control) != 0 || (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
            }
        }

        /// <summary>
        /// Stores reference to previously selected area.
        /// </summary>
        Rect PreviousArea;

        /// <summary>
        /// Point indicating where the drag started.
        /// </summary>
        Point StartPosition;

        #endregion

        #region Public

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

        #endregion

        #endregion

        #region Methods

        #region Private

        void AdjustScroll(Point Position)
        {
            double HorizontalPosition = Position.X, VerticalPosition = Position.Y;

            var ScrollOffset = ItemsControlExtensions.GetDragScrollOffset(this.ItemsControl);
            var ScrollTolerance = ItemsControlExtensions.GetDragScrollTolerance(this.ItemsControl);

            //Cursor is at top, scroll up.
            if (VerticalPosition < ScrollTolerance)
                this.PART_ScrollViewer.ScrollToVerticalOffset(this.PART_ScrollViewer.VerticalOffset - ScrollOffset);
            //Cursor is at bottom, scroll down.  
            else if (VerticalPosition > this.ItemsControl.ActualHeight - ScrollTolerance) //Bottom of visible list?
                this.PART_ScrollViewer.ScrollToVerticalOffset(this.PART_ScrollViewer.VerticalOffset + ScrollOffset);
            //Cursor is at left, scroll left.  
            else if (HorizontalPosition < ScrollTolerance)
                this.PART_ScrollViewer.ScrollToHorizontalOffset(this.PART_ScrollViewer.HorizontalOffset - ScrollOffset);
            //Cursor is at right, scroll right.  
            else if (HorizontalPosition > this.ItemsControl.ActualWidth - ScrollTolerance)
                this.PART_ScrollViewer.ScrollToHorizontalOffset(this.PART_ScrollViewer.HorizontalOffset + ScrollOffset);
        }

        /// <summary>
        /// Ensures given point values do not exceed canvas bounds.
        /// </summary>
        Point BoundPosition(Point Position, Size Selection, Size Bounds)
        {
            Position.X = Position.X < 0 ? 0 : Position.X;
            Position.Y = Position.Y < 0 ? 0 : Position.Y;

            var x = Bounds.Width - Selection.Width;
            var y = Bounds.Height - Selection.Height;

            Position.X = Position.X > x ? x : Position.X;
            Position.Y = Position.Y > y ? y : Position.Y;

            return new Point(Position.X, Position.Y);
        }

        /// <summary>
        /// Ensures given size does not exceed canvas bounds.
        /// </summary>
        Size BoundSize(Rect Rect, Size Bounds)
        {
            var Width =
                Rect.Width + Rect.X > Bounds.Width
                    ? Bounds.Width - Rect.X
                    : Rect.Width;
            var Height =
                Rect.Height + Rect.Y > Bounds.Height
                    ? Bounds.Height - Rect.Y
                    : Rect.Height;

            return new Size(Width.Coerce(0.0, true), Height.Coerce(0.0, true));
        }

        /// <summary>
        /// Find ScrollContentPresenter element in ScrollViewer template.
        /// </summary>
        void FindPresenter()
        {
            if (this.PART_ScrollViewer != null && this.PART_ScrollViewer.Style != null && (this.Hash == null || this.PART_ScrollViewer.Style.GetHashCode() != this.Hash.Value))
            {
                foreach (var Element in this.PART_ScrollViewer.GetVisualChildren<FrameworkElement>())
                {
                    if (Element.Is<ScrollContentPresenter>())
                    {
                        this.ScrollContentPresenter = Element as ScrollContentPresenter;
                        this.Hash = this.PART_ScrollViewer.Style.GetHashCode();
                        break;
                    }
                }
            }
        }

        async Task<Rect> GetRect(Point CurrentPosition)
        {
            var Result = new Rect();

            var StartPosition = this.StartPosition;

            double
                x = (StartPosition.X < CurrentPosition.X ? StartPosition.X : CurrentPosition.X),
                y = (StartPosition.Y < CurrentPosition.Y ? StartPosition.Y : CurrentPosition.Y);

            var SelectionSize = new Size(this.Selection.Width, this.Selection.Height);
            var ScrollContentPresenterSize = new Size(this.ScrollContentPresenter.Width, this.ScrollContentPresenter.Height);

            await Task.Run(new Action(() =>
            {
                Result.Size = new Size(Math.Abs(CurrentPosition.X - StartPosition.X), Math.Abs(CurrentPosition.Y - StartPosition.Y));

                var Point = BoundPosition(new Point(x, y), SelectionSize, ScrollContentPresenterSize);
                Result.X = Point.X;
                Result.Y = Point.Y;

                Result.Size = this.BoundSize(Result, ScrollContentPresenterSize);
            }));

            return Result;
        }

        void OnDragSelectStarted(MouseButtonEventArgs e)
        {
            this.PART_Grid.CaptureMouse();

            this.IsDragging = true;

            this.PreviousArea = new Rect();
            this.StartPosition = e.GetPosition(this.PART_Grid);

            this.Selection.Set(this.StartPosition.X, this.StartPosition.Y, 0, 0);
        }

        async Task OnDragSelect(MouseEventArgs e)
        {
            var Rect = await GetRect(e.GetPosition(PART_Grid));
            if (IsDragging)
            {
                Selection.Set(Rect.X, Rect.Y, Rect.Width, Rect.Height);
                Select(ItemsControl, new Rect(ScrollContentPresenter.TranslatePoint(Selection.TopLeft, ScrollContentPresenter), ScrollContentPresenter.TranslatePoint(Selection.BottomRight, ScrollContentPresenter)));
                AdjustScroll(e.GetPosition(ItemsControl));
            }
        }

        void OnDragSelectEnded(MouseButtonEventArgs e)
        {
            this.IsDragging = false;

            if (this.PART_Grid.IsMouseCaptured)
                this.PART_Grid.ReleaseMouseCapture();

            this.Selection.Width = 0;
            this.Selection.Height = 0;

            this.StartPosition = default(Point);
        }

        void OnInitialized()
        {
            PART_Grid = ItemsControl.Template.FindName("PART_Grid", ItemsControl) as Grid;
            if (PART_Grid == null)
                throw new KeyNotFoundException("Grid cannot be null.");

            //Required for mouse events
            PART_Grid.Background = Brushes.Transparent;

            PART_ScrollViewer = ItemsControl.Template.FindName("PART_ScrollViewer", ItemsControl) as ScrollViewer;
            if (PART_ScrollViewer == null)
                throw new KeyNotFoundException("ScrollViewer cannot be null.");

            Selection = new Selection(0.0, 0.0, 0.0, 0.0);

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

        /// <summary>
        /// Updates selection when drag selection is enabled and active.
        /// </summary>
        void Select(ItemsControl Control, Rect Area)
        {
            //Check if selection should be appended are replaced. We allow appending when either control or shift key is pressed.
            if (!this.IsModifierKeyPressed)
                this.ItemsControl.TryClearSelection();

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

                if (IsSelected != null)
                {
                    if (Control is TreeView || Control is TreeViewItem)
                    {
                        var TreeViewItem = Item as TreeViewItem;
                        TreeViewItemExtensions.SetIsSelected(TreeViewItem, IsSelected.Value);
                        if (TreeViewItem.IsExpanded && TreeViewItem.Items.Count > 0)
                            this.Select(TreeViewItem, Area);
                    }
                    else this.Select(Item, IsSelected.Value);
                }
            }
            this.PreviousArea = Area;
        }

        /// <summary>
        /// Attempts to select or deselect a ListViewItem.
        /// </summary>
        bool Select(DependencyObject Item, bool IsSelected)
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

        #region Public

        public void Register()
        {
            this.PART_Grid.MouseDown += this.OnMouseDown;
            this.PART_Grid.MouseMove += this.OnMouseMove;
            this.PART_Grid.MouseUp += this.OnMouseUp;

            this.PART_ScrollViewer.LayoutUpdated += FindPresenter;
            this.PART_ScrollViewer.Loaded += FindPresenter;
        }

        public void Deregister()
        {
            this.PART_Grid.MouseDown -= this.OnMouseDown;
            this.PART_Grid.MouseMove -= this.OnMouseMove;
            this.PART_Grid.MouseUp -= this.OnMouseUp;

            this.PART_ScrollViewer.LayoutUpdated -= FindPresenter;
            this.PART_ScrollViewer.Loaded -= FindPresenter;
        }

        #endregion

        #region Events

        void FindPresenter(object sender, EventArgs e)
        {
            this.FindPresenter();
        }

        /// <summary>
        /// Occurs when mouse is down; begins drag.
        /// </summary>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && !this.HasSingleSelectionMode)
                this.OnDragSelectStarted(e);
        }

        /// <summary>
        /// Ocurrs whenever mouse moves; drag is evaluated.
        /// </summary>
        async void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsDragging)
                await this.OnDragSelect(e);
        }

        /// <summary>
        /// Occurs when mouse is up; ends drag.
        /// </summary>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && this.IsDragging)
                this.OnDragSelectEnded(e);
        }

        #endregion

        #endregion

        #region DragSelectionManager

        public DragSelectionManager(ItemsControl ItemsControl) : base()
        {
            this.ItemsControl = ItemsControl;
            this.OnInitialized();
        }

        #endregion
    }
}
