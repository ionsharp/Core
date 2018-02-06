using Imagin.Controls.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TreeViewColumnHeadersPresenter : ResizableGrid
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CanResizeColumnsProperty = DependencyProperty.Register("CanResizeColumns", typeof(bool), typeof(TreeViewColumnHeadersPresenter), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(TreeViewColumnCollection), typeof(TreeViewColumnHeadersPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColumnsChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Child"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnColumnsChanged(TreeViewColumnCollection Value)
        {
            this.ItemsSource = Value;
        }

        #endregion

        #region TreeViewColumnHeadersPresenter

        /// <summary>
        /// 
        /// </summary>
        public TreeViewColumnHeadersPresenter() : base()
        {
        }

        #endregion
    }
}
