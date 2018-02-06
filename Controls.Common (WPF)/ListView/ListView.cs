using Imagin.Common.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ListView : System.Windows.Controls.ListView
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty<List<object>, ListView> SelectedValuesProperty = new DependencyProperty<List<object>, ListView>("SelectedValues", new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public List<object> SelectedValues
        {
            get
            {
                return SelectedValuesProperty.Get(this);
            }
            set
            {
                SelectedValuesProperty.Set(this, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ListView() : base()
        {
            SelectionChanged += ListView_SelectionChanged;
        }

        void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCurrentValue(SelectedValuesProperty.Property, e.AddedItems.ToList());
        }
    }
}
