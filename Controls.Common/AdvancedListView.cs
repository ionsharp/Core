using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public class AdvancedListView : ListView, IDragSelector
    {
        #region Properties

        public static DependencyProperty DragScrollOffsetProperty = DependencyProperty.Register("DragScrollOffset", typeof(double), typeof(AdvancedListView), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Indicates offset to apply when automatically scrolling.
        /// </summary>
        public double DragScrollOffset
        {
            get
            {
                return (double)GetValue(DragScrollOffsetProperty);
            }
            set
            {
                SetValue(DragScrollOffsetProperty, value);
            }
        }

        public static DependencyProperty DragScrollToleranceProperty = DependencyProperty.Register("DragScrollTolerance", typeof(double), typeof(AdvancedListView), new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Indicates how far from outer width/height to allow offset application when automatically scrolling.
        /// </summary>
        public double DragScrollTolerance
        {
            get
            {
                return (double)GetValue(DragScrollToleranceProperty);
            }
            set
            {
                SetValue(DragScrollToleranceProperty, value);
            }
        }

        public static DependencyProperty DragSelectionProperty = DependencyProperty.Register("DragSelection", typeof(Selection), typeof(AdvancedListView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Selection DragSelection
        {
            get
            {
                return (Selection)GetValue(DragSelectionProperty);
            }
            set
            {
                SetValue(DragSelectionProperty, value);
            }
        }

        public static DependencyProperty DragSelectorProperty = DependencyProperty.Register("DragSelector", typeof(DragSelector), typeof(AdvancedListView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DragSelector DragSelector
        {
            get
            {
                return (DragSelector)GetValue(DragSelectorProperty);
            }
            set
            {
                SetValue(DragSelectorProperty, value);
            }
        }

        public static DependencyProperty IsDragSelectionEnabledProperty = DependencyProperty.Register("IsDragSelectionEnabled", typeof(bool), typeof(AdvancedListView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Enables selecting items while dragging with a rectangular selector.
        /// </summary>
        public bool IsDragSelectionEnabled
        {
            get
            {
                return (bool)GetValue(IsDragSelectionEnabledProperty);
            }
            set
            {
                SetValue(IsDragSelectionEnabledProperty, value);
            }
        }

        public static DependencyProperty ScrollWrapProperty = DependencyProperty.Register("ScrollWrap", typeof(bool), typeof(AdvancedListView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Determines whether or not selections should "wrap" to beginning or end.
        /// </summary>
        public bool ScrollWrap
        {
            get
            {
                return (bool)GetValue(ScrollWrapProperty);
            }
            set
            {
                SetValue(ScrollWrapProperty, value);
            }
        }

        #endregion

        #region Methods

        #region Private

        /// <summary>
        /// Moves current selection to the left.
        /// </summary>
        void MoveLeft()
        {
            //If nothing is selected, moving left selects first item.
            if (this.SelectedItems.Count == 0)
                this.Items.MoveCurrentToFirst();
            else if (!this.Items.MoveCurrentToPrevious())
            {
                //If that fails, there is no previous, we are already at first.
                //Attempt to select last (if allowed).
                if (this.ScrollWrap)
                    this.Items.MoveCurrentToLast();
            }
        }

        /// <summary>
        /// Moves current selection to the right.
        /// </summary>
        void MoveRight()
        {
            //If nothing is selected, moving right selects last item.
            if (this.SelectedItems.Count == 0)
                this.Items.MoveCurrentToLast();
            //Otherwise, attempt to select next.
            else if (!this.Items.MoveCurrentToNext())
            {
                //If that fails, there is no next, we are already at last.
                //Attempt to select first (if allowed).
                if (this.ScrollWrap)
                    this.Items.MoveCurrentToFirst();
            }
        }

        /// <summary>
        /// Moves current selection to next row.
        /// </summary>
        void MoveToNextRow(string CompassDirection, int ItemsPerRow, int Offset = 1)
        {
            int? NewIndex = null;
            if (CompassDirection == "Up")
            {
                NewIndex = this.SelectedIndex - ItemsPerRow + Offset;
                if (NewIndex < 0)
                {
                    //NewIndex = this.ScrollWrap ? this.Items.Count + NewIndex : null;
                }
            }
            else if (CompassDirection == "Down")
            {
                NewIndex = this.SelectedIndex + ItemsPerRow - Offset;
                if (NewIndex >= this.Items.Count)
                {
                    //NewIndex = this.ScrollWrap ? NewIndex - this.Items.Count : null;
                }
            }
            //If a new index was successfully calculated
            if (NewIndex != null)
            {
                //Double check it's valid.
                NewIndex = NewIndex < 0 ? 0 : (NewIndex >= this.Items.Count ? this.Items.Count - 1 : NewIndex);
                this.SelectedIndex = NewIndex.Value;
            }
        }

        #endregion

        #region Override

        /// <summary>
        /// Occurs when template is applied; gets references to 
        /// elements contained in template and registers mouse events
        /// for dragging.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
            this.DragSelector = new DragSelector(this);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Makes selection based on CompassDirectional key pressed.
        /// </summary>
        /// <pseudo>
        /// If up or left is clicked and nothing is selected, 
        /// select first. If bottom or right is clicked and 
        /// nothing is selected, select last. If first is 
        /// selected and clicking left or up, select last. 
        /// If last is selected and clicking right or down, 
        /// select first.
        /// </summary>
        public static readonly RoutedUICommand MakeSelection = new RoutedUICommand("MakeSelection", "MakeSelection", typeof(AdvancedListView));
        void MakeSelection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Can't select nothing so do nothing.
            if (this.Items.Count == 0)
                return;
            string CompassDirection = e.Parameter.ToString();
            switch (CompassDirection)
            {
                case "Up":
                case "Down":
                    double ListViewWidth = this.ActualWidth - this.Padding.Left - this.Padding.Right;
                    double SelectedItemWidth = ((ListViewItem)this.ItemContainerGenerator.ContainerFromItem(this.Items.CurrentItem)).ActualWidth;
                    int ItemsPerRow = Convert.ToInt32(Math.Round(ListViewWidth / SelectedItemWidth));
                    //If there's only one item in each row, 
                    //we don't have to calculate anything.
                    //In this scenario, up corresponds to left, 
                    //and  down corresponds to right.
                    if (ItemsPerRow <= 1)
                    {
                        if (CompassDirection == "Up")
                            this.MoveLeft();
                        else if (CompassDirection == "Down")
                            this.MoveRight();
                        return;
                    }
                    this.MoveToNextRow(CompassDirection, ItemsPerRow);
                    break;
                case "Left":
                    this.MoveLeft();
                    break;
                case "Right":
                    this.MoveRight();
                    break;
            }
        }

        #endregion

        #endregion

        #region CustomListView

        public AdvancedListView() : base()
        {
            this.DefaultStyleKey = typeof(AdvancedListView);

            //Enables moving current selection around
            this.IsSynchronizedWithCurrentItem = true;

            this.CommandBindings.Add(new CommandBinding(MakeSelection, this.MakeSelection_Executed));

            this.InputBindings.Add(new KeyBinding(MakeSelection, Key.Up, ModifierKeys.None)
            {
                CommandParameter = "Up"
            });
            this.InputBindings.Add(new KeyBinding(MakeSelection, Key.Down, ModifierKeys.None)
            {
                CommandParameter = "Down"
            });
            this.InputBindings.Add(new KeyBinding(MakeSelection, Key.Left, ModifierKeys.None)
            {
                CommandParameter = "Left"
            });
            this.InputBindings.Add(new KeyBinding(MakeSelection, Key.Right, ModifierKeys.None)
            {
                CommandParameter = "Right"
            });
        }

        #endregion
    }
}
