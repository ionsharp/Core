using System.Collections;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DataGrid : System.Windows.Controls.DataGrid
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IList), typeof(DataGrid), new FrameworkPropertyMetadata(default(IList), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public new IList SelectedItems
        {
            get
            {
                return (IList)GetValue(SelectedItemsProperty);
            }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataGrid() : base()
        {
            SelectionChanged += OnSelectionChanged;
        }

        void OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            OnSelectedItemsChanged(e.AddedItems);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSelectedItemsChanged(IList Value)
        {
            SetCurrentValue(SelectedItemsProperty, Value);
        }
    }
}
