using Imagin.Controls.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    public class TreeViewColumnHeadersPresenter : ResizableGrid
    {
        #region Properties

        public static DependencyProperty CanResizeColumnsProperty = DependencyProperty.Register("CanResizeColumns", typeof(bool), typeof(TreeViewColumnHeadersPresenter), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanResizeColumns
        {
            get
            {
                return (bool)GetValue(CanResizeColumnsProperty);
            }
            set
            {
                SetValue(CanResizeColumnsProperty, value);
            }
        }

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(TreeViewColumnCollection), typeof(TreeViewColumnHeadersPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColumnsChanged));
        public TreeViewColumnCollection Columns
        {
            get
            {
                return (TreeViewColumnCollection)GetValue(ColumnsProperty);
            }
            set
            {
                SetValue(ColumnsProperty, value);
            }
        }
        static void OnColumnsChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((TreeViewColumnHeadersPresenter)Object).OnColumnsChanged((TreeViewColumnCollection)e.NewValue);
        }

        #endregion

        #region Methods

        protected override ColumnDefinition GetColumnDefinition(ContentPresenter Child, int Index)
        {
            var Result = base.GetColumnDefinition(Child, Index);
            if (Child.Content != null && Child.Content is TreeViewColumn)
            {
                var Column = Child.Content as TreeViewColumn;
                Result = new ColumnDefinition();
                BindingOperations.SetBinding(Result, ColumnDefinition.WidthProperty, new Binding()
                {
                    Path = new PropertyPath("Width"),
                    Mode = BindingMode.TwoWay,
                    Source = Column
                });
                BindingOperations.SetBinding(Result, ColumnDefinition.MinWidthProperty, new Binding()
                {
                    Path = new PropertyPath("MinWidth"),
                    Mode = BindingMode.TwoWay,
                    Source = Column
                });
                BindingOperations.SetBinding(Result, ColumnDefinition.MaxWidthProperty, new Binding()
                {
                    Path = new PropertyPath("MaxWidth"),
                    Mode = BindingMode.TwoWay,
                    Source = Column
                });
                BindingOperations.SetBinding(Child, ContentPresenter.VisibilityProperty, new Binding()
                {
                    Converter = new BooleanToVisibilityConverter(),
                    Path = new PropertyPath("(0)", DependencyObjectExtensions.IsVisibleProperty),
                    Mode = BindingMode.OneWay,
                    Source = Column
                });
            }
            return Result;
        }

        protected virtual void OnColumnsChanged(TreeViewColumnCollection Value)
        {
            this.ItemsSource = Value;
        }

        #endregion

        #region TreeViewColumnHeadersPresenter

        public TreeViewColumnHeadersPresenter() : base()
        {
        }

        #endregion
    }
}
