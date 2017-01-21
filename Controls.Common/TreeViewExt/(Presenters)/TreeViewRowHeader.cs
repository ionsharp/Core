using Imagin.Common.Extensions;
using Imagin.Controls.Common.Converters;
using Imagin.Controls.Common.Extensions;
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
    /// This is only needed if item is hosted in TreeViewExt
    /// and, therefore, supports showing multiple columns. If 
    /// columns should be hidden or otherwise aren't present,
    /// the item's header is shown by default.
    /// </remarks>
    public class TreeViewRowHeader : ResizableGrid
    {
        bool isInitialized = false;

        TreeView PART_TreeView { get; set; } = null;

        internal static DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(TreeViewItem), typeof(TreeViewRowHeader), new FrameworkPropertyMetadata(null));
        internal TreeViewItem PART_TreeViewItem
        {
            get
            {
                return (TreeViewItem)GetValue(ItemProperty);
            }
            set
            {
                SetValue(ItemProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnOffsetProperty = DependencyProperty.Register("ColumnOffset", typeof(double), typeof(TreeViewRowHeader), new FrameworkPropertyMetadata(25.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// Get content for specified item based on specified column.
        /// </summary>
        object GetColumnContent(TreeViewColumn Column, TreeViewItem Item)
        {
            var a = default(DependencyObject);

            if (Column.Is<TreeViewTemplateColumn>())
            {
                a = new ContentControl()
                {
                    Content = Item.DataContext,
                    ContentTemplate = Column.As<TreeViewTemplateColumn>().Template
                };
            }
            else if (Column.Is<TreeViewTextColumn>())
            {
                var TextColumn = Column as TreeViewTextColumn;
                a = new TextBlock()
                {
                    TextTrimming = TextColumn.TextTrimming
                };
                BindingOperations.SetBinding(a.As<DependencyObject>(), TextBlock.TextProperty, new Binding()
                {
                    Converter = TextColumn.Converter,
                    Path = new PropertyPath(TextColumn.MemberPath),
                    Mode = BindingMode.OneWay,
                    Source = Item.DataContext
                });
            }

            BindingOperations.SetBinding(a, FrameworkElement.VisibilityProperty, new Binding()
            {
                Converter = new BooleanToVisibilityConverter(),
                Path = new PropertyPath("(0)", ControlExtensions.IsVisibleProperty),
                Mode = BindingMode.OneWay,
                Source = Column
            });
            a.Bind(FrameworkElement.HorizontalAlignmentProperty, Column, "HorizontalContentAlignment", BindingMode.OneWay);
            a.Bind(FrameworkElement.VerticalAlignmentProperty, Column, "VerticalContentAlignment", BindingMode.OneWay);

            return a;
        }

        /// <summary>
        /// If not hosted in TreeViewExt or shouldn't show multiple columns, display default header.
        /// </summary>
        object GetDefaultHeader(TreeViewItem Item)
        {
            return new ContentPresenter()
            {
                Content = Item.Header,
                ContentStringFormat = Item.HeaderStringFormat,
                ContentTemplate = Item.HeaderTemplate,
                ContentTemplateSelector = Item.HeaderTemplateSelector,
            };
        }

        /// <summary>
        /// Gets header for the specified item; columns are optional.
        /// </summary>
        IEnumerable GetHeader(TreeView TreeView, TreeViewItem TreeViewItem)
        {
            var Source = new ObservableCollection<object>();
            if (TreeView is TreeViewExt && TreeView.As<TreeViewExt>().Columns.Count > 0)
            {
                var t = TreeView as TreeViewExt;
                t.Columns.For(0, t.Columns.Count, (a, b) => Source.Add(GetColumnContent(a[b], TreeViewItem)));
            }
            else
            {
                Source.Add(GetDefaultHeader(TreeViewItem));
            }
            return Source;
        }

        /// <summary>
        /// Hide the item while fetching the header; display item when finished. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!isInitialized)
            {
                isInitialized = true;

                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(async () =>
                {
                    PART_TreeViewItem = this.GetVisualParent<TreeViewItem>();

                    if (PART_TreeViewItem == null)
                        throw new InvalidOperationException("TreeViewRowHeader must be hosted in a control that inherits TreeViewItem.");

                    PART_TreeViewItem.Visibility = Visibility.Collapsed;
                    PART_TreeViewItem.Opacity = 0;

                    PART_TreeView = PART_TreeViewItem.GetParent<TreeView>();

                    ItemsSource = GetHeader(PART_TreeView, PART_TreeViewItem);

                    PART_TreeViewItem.Visibility = Visibility.Visible;
                    await PART_TreeViewItem.FadeIn();
                }));
            }
        }

        protected override ColumnDefinition GetColumnDefinition(ContentPresenter Child, int Index)
        {
            var Result = base.GetColumnDefinition(Child, Index);

            var TreeViewItem = PART_TreeViewItem ?? this.GetParent<TreeViewItem>();
            if (TreeViewItem == null)
                throw new InvalidOperationException("TreeViewRowHeader must be hosted in a control that inherits TreeViewItem.");

            var TreeView = PART_TreeView ?? TreeViewItem.GetParent<TreeView>();

            try
            {
                if (TreeView is TreeViewExt && TreeView.As<TreeViewExt>().Columns.Count > 0 && Index < TreeView.As<TreeViewExt>().Columns.Count)
                {
                    var Column = TreeView.As<TreeViewExt>().Columns[Index];

                    Result = new ColumnDefinition();

                    var NodeDepth = TreeViewItem.GetDepth();

                    var a = Index == 0 ? new TreeViewColumnGridLengthConverter(ColumnOffset) : null;
                    var b = Index == 0 ? new TreeViewColumnDoubleConverter(ColumnOffset) : null;
                    var c = Index == 0 ? new TreeViewColumnDoubleConverter(ColumnOffset) : null;

                    var n = Index == 0 ? NodeDepth.ToDouble() as object : null;

                    Result.Bind(ColumnDefinition.WidthProperty, Column, "Width", a, n, BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                    Result.Bind(ColumnDefinition.MinWidthProperty, Column, "MinWidth", b, n, BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
                    Result.Bind(ColumnDefinition.MaxWidthProperty, Column, "MaxWidth", c, n, BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);

                    Child.Bind(ContentPresenter.MarginProperty, Column, "ContentMargin");
                }
            }
            catch
            {
                Result = null;
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeViewRowHeader() : base()
        {
            ShowSplitter = false;
            Loaded += OnLoaded;

            this.Bind(TreeViewRowHeader.ItemProperty, ".", RelativeSourceMode.TemplatedParent);
        }
    }
}
