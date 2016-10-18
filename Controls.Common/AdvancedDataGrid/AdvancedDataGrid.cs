using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    public class AdvancedDataGrid : DataGrid, IDragSelector
    {
        #region Properties

        public static DependencyProperty AutoSizeColumnsProperty = DependencyProperty.Register("AutoSizeColumns", typeof(bool), typeof(AdvancedDataGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAutoSizeColumnsChanged));
        public bool AutoSizeColumns
        {
            get
            {
                return (bool)GetValue(AutoSizeColumnsProperty);
            }
            set
            {
                SetValue(AutoSizeColumnsProperty, value);
            }
        }
        private static void OnAutoSizeColumnsChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            (Object as AdvancedDataGrid).SetColumnWidth();
        }

        public static DependencyProperty DragSelectorProperty = DependencyProperty.Register("DragSelector", typeof(DragSelector), typeof(AdvancedDataGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty IsDragSelectionEnabledProperty = DependencyProperty.Register("IsDragSelectionEnabled", typeof(bool), typeof(AdvancedDataGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty DragScrollOffsetProperty = DependencyProperty.Register("DragScrollOffset", typeof(double), typeof(AdvancedDataGrid), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty DragScrollToleranceProperty = DependencyProperty.Register("DragScrollTolerance", typeof(double), typeof(AdvancedDataGrid), new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty DragSelectionProperty = DependencyProperty.Register("DragSelection", typeof(Selection), typeof(AdvancedDataGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ScrollAddedIntoViewProperty = DependencyProperty.Register("ScrollAddedIntoView", typeof(bool), typeof(AdvancedDataGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ScrollAddedIntoView
        {
            get
            {
                return (bool)GetValue(ScrollAddedIntoViewProperty);
            }
            set
            {
                SetValue(ScrollAddedIntoViewProperty, value);
            }
        }

        #endregion

        #region Methods

        Style GetColumnHeaderStyle()
        {
            ContextMenu ColumnHeaderContextMenu = new ContextMenu();
            foreach (var Column in this.Columns)
            {
                if (!(Column is DataGridTextColumn) && !(Column is DataGridTemplateColumn)) continue; //Make sure it's supported column types

                if ((Column.Header == null || Column.Header.ToString() == string.Empty) && (Column.SortMemberPath == null || Column.SortMemberPath == string.Empty)) continue; //If both properties are empty, skip the column. Column.Header has priority over Column.SortMemberPath.
                MenuItem Item = this.GenerateMenuItem(Column.Header == null ? Column.SortMemberPath : Column.Header.ToString());
                Binding Binding = this.GetBinding(Item);
                BindingOperations.SetBinding(Column, DataGridTextColumn.VisibilityProperty, Binding);
                ColumnHeaderContextMenu.Items.Add(Item);
                if (this.AutoSizeColumns) Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            Style NewStyle = null;
            try
            {
                var BaseStyle = this.ColumnHeaderStyle;
                NewStyle = new Style(typeof(DataGridColumnHeader), (Style)BaseStyle);
            }
            catch
            {
                NewStyle = new Style(typeof(DataGridColumnHeader));
            }
            NewStyle.Setters.Add(new Setter(DataGridColumnHeader.ContextMenuProperty, ColumnHeaderContextMenu));
            return NewStyle;
        }

        MenuItem GenerateMenuItem(string Header)
        {
            return new MenuItem()
            {
                Header = Header, //If Header is empty, fall back on SortMemberPath. This happens when we don't want to display the header in the DataGrid, but do in the ContextMenu.
                IsCheckable = true,
                IsChecked = true,
                StaysOpenOnClick = true
            };
        }

        Binding GetBinding(object Source)
        {
            Binding Binding = new Binding();
            Binding.Source = Source;
            Binding.Path = new PropertyPath("IsChecked");
            Binding.Mode = BindingMode.TwoWay;
            Binding.Converter = new BooleanToVisibilityConverter();
            Binding.BindsDirectlyToSource = true;
            return Binding;
        }

        void SetColumnWidth()
        {
            foreach (DataGridColumn Column in this.Columns)
            {
                if (this.AutoSizeColumns)
                    Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                else
                    Column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            }
        }

        void SetStyle()
        {
            if (this.Columns == null || this.Columns.Count == 0) return;
            Style DataGridStyle = new Style(typeof(AdvancedDataGrid), (Style)FindResource(typeof(AdvancedDataGrid)));
            DataGridStyle.Setters.Add(new Setter(AdvancedDataGrid.ColumnHeaderStyleProperty, this.GetColumnHeaderStyle()));
            this.Style = DataGridStyle;
        }

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
            this.DragSelector = new DragSelector(this);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (ScrollAddedIntoView && this.Items.Count > 0) this.ScrollIntoView(this.Items[this.Items.Count - 1]);
        }

        #endregion

        #endregion

        #region AdvancedDataGrid

        public AdvancedDataGrid() : base()
        {
            this.DefaultStyleKey = typeof(AdvancedDataGrid);
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SetStyle();
        }

        #endregion
    }
}
