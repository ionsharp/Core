using Imagin.Common.Extensions;
using Imagin.Controls.Common.Converters;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A container used to present a TreeViewItem's header.
    /// </summary>
    /// <remarks>
    /// This is only needed if item is hosted in AdvancedTreeView
    /// and, therefore, supports showing multiple columns. If 
    /// columns should be hidden or otherwise aren't present,
    /// the item's header is shown by default.
    /// </remarks>
    public class TreeViewItemHeaderPresenter : ResizableGrid
    {
        #region Properties

        bool isInitialized = false;

        public static DependencyProperty ColumnOffsetProperty = DependencyProperty.Register("ColumnOffset", typeof(double), typeof(TreeViewItemHeaderPresenter), new FrameworkPropertyMetadata(25.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double ColumnOffset
        {
            get
            {
                return (double)GetValue(ColumnOffsetProperty);
            }
            set
            {
                SetValue(ColumnOffsetProperty, value);
            }
        }

        AdvancedTreeView PART_AdvancedTreeView
        {
            get; set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get content for specified item based on specified column.
        /// </summary>
        object GetColumnContent(TreeViewColumn Column, TreeViewItem AssociatedItem)
        {
            object Content = null;
            if (Column.Is<TreeViewTemplateColumn>())
            {
                Content = new ContentControl()
                {
                    Content = AssociatedItem.DataContext,
                    ContentTemplate = Column.As<TreeViewTemplateColumn>().Template
                };
            }
            else if (Column.Is<TreeViewTextColumn>())
            {
                var TextColumn = Column as TreeViewTextColumn;
                Content = new TextBlock()
                {
                    TextTrimming = TextColumn.TextTrimming
                };
                BindingOperations.SetBinding(Content.As<DependencyObject>(), TextBlock.TextProperty, new Binding()
                {
                    Converter = TextColumn.Converter,
                    Path = new PropertyPath(TextColumn.MemberPath),
                    Mode = BindingMode.OneWay,
                    Source = AssociatedItem.DataContext
                });
            }
            BindingOperations.SetBinding(Content.As<DependencyObject>(), FrameworkElement.VisibilityProperty, new Binding()
            {
                Converter = new BooleanToVisibilityConverter(),
                Path = new PropertyPath("IsVisible"),
                Mode = BindingMode.OneWay,
                Source = Column
            });
            BindingOperations.SetBinding(Content.As<DependencyObject>(), FrameworkElement.HorizontalAlignmentProperty, new Binding()
            {
                Path = new PropertyPath("HorizontalContentAlignment"),
                Mode = BindingMode.OneWay,
                Source = Column
            });
            BindingOperations.SetBinding(Content.As<DependencyObject>(), FrameworkElement.VerticalAlignmentProperty, new Binding()
            {
                Path = new PropertyPath("VerticalContentAlignment"),
                Mode = BindingMode.OneWay,
                Source = Column
            });
            return Content;
        }

        /// <summary>
        /// If not hosted in AdvancedTreeView or shouldn't show multiple columns, display default header.
        /// </summary>
        object GetDefaultHeader(TreeViewItem Item)
        {
            return new ObservableCollection<object>(new object[]
            {
                //Check if a default template has been specified; use item's default header if not.
                this.PART_AdvancedTreeView.ItemTemplate == null && this.PART_AdvancedTreeView.ItemTemplateSelector == null
                ? Item.Header
                : new ContentControl()
                    {
                        Content = Item.DataContext,
                        ContentStringFormat = this.PART_AdvancedTreeView.ItemStringFormat,
                        ContentTemplate = this.PART_AdvancedTreeView.ItemTemplate,
                        ContentTemplateSelector = this.PART_AdvancedTreeView.ItemTemplateSelector
                    }
            });
        }

        /// <summary>
        /// Gets header for the specified item; columns are optional.
        /// </summary>
        IEnumerable GetHeader(TreeView TreeView, TreeViewItem TreeViewItem)
        {
            var Source = new ObservableCollection<object>();
            if (TreeViewItem != null && TreeView != null && TreeView is AdvancedTreeView)
            {
                var AdvancedTreeView = TreeView as AdvancedTreeView;
                if (AdvancedTreeView.Columns != null && AdvancedTreeView.Columns.Count > 0)
                {
                    for (int i = 0; i < AdvancedTreeView.Columns.Count; i++)
                        Source.Add(this.GetColumnContent(AdvancedTreeView.Columns[i], TreeViewItem));
                }
            }
            if (Source.Count == 0)
                Source.Add(this.GetDefaultHeader(TreeViewItem));
            return Source;
        }

        void OnLoaded()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                DependencyObject Parent = this;
                var TreeViewItem = this.GetParent<TreeViewItem>();
                if (TreeViewItem != null)
                {
                    if (PART_AdvancedTreeView == null)
                        PART_AdvancedTreeView = TreeViewItem.GetParent<AdvancedTreeView>();
                    this.ItemsSource = this.GetHeader(PART_AdvancedTreeView, TreeViewItem);
                }
            }));
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!this.isInitialized)
            {
                this.isInitialized = true;
                this.OnLoaded();
            }
        }

        protected override ColumnDefinition GetColumnDefinition(ContentPresenter Child, int Index)
        {
            var Result = base.GetColumnDefinition(Child, Index);

            var TreeViewItem = this.GetParent<TreeViewItem>();
            var AdvancedTreeView = TreeViewItem.GetParent<AdvancedTreeView>();
            try
            {
                if (AdvancedTreeView != null && AdvancedTreeView.Columns.Count > 0 && Index < AdvancedTreeView.Columns.Count)
                {
                    var Column = AdvancedTreeView.Columns[Index];

                    Result = new ColumnDefinition();

                    var NodeDepth = TreeViewItem.GetDepth();
                    BindingOperations.SetBinding(Result, ColumnDefinition.WidthProperty, new Binding()
                    {
                        Converter = Index == 0 ? new TreeViewColumnGridLengthConverter(ColumnOffset) : null,
                        ConverterParameter = Index == 0 ? NodeDepth.ToDouble() as object : null,
                        Path = new PropertyPath("Width"),
                        Mode = BindingMode.TwoWay,
                        Source = Column,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    BindingOperations.SetBinding(Result, ColumnDefinition.MinWidthProperty, new Binding()
                    {
                        Converter = Index == 0 ? new TreeViewColumnDoubleConverter(ColumnOffset) : null,
                        ConverterParameter = Index == 0 ? NodeDepth.ToDouble() as object : null,
                        Path = new PropertyPath("MinWidth"),
                        Mode = BindingMode.TwoWay,
                        Source = Column,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    BindingOperations.SetBinding(Result, ColumnDefinition.MaxWidthProperty, new Binding()
                    {
                        Converter = Index == 0 ? new TreeViewColumnDoubleConverter(ColumnOffset) : null,
                        ConverterParameter = Index == 0 ? NodeDepth.ToDouble() as object : null,
                        Path = new PropertyPath("MaxWidth"),
                        Mode = BindingMode.TwoWay,
                        Source = Column,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    BindingOperations.SetBinding(Child, ContentPresenter.MarginProperty, new Binding()
                    {
                        Path = new PropertyPath("ContentMargin"),
                        Mode = BindingMode.TwoWay,
                        Source = Column
                    });
                }
            }
            catch
            {
                return Result = null;
            }
            return Result;
        }

        #endregion

        #region TreeViewItemHeaderPresenter

        public TreeViewItemHeaderPresenter() : base()
        {
            this.ShowSplitter = false;
            this.Loaded += OnLoaded;
        }

        #endregion
    }
}
