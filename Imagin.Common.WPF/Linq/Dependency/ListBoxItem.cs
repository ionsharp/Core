using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(ListBoxItem))]
    public static class XListBoxItem
    {
        #region LastSelected

        public static readonly DependencyProperty LastSelectedProperty = DependencyProperty.RegisterAttached("LastSelected", typeof(bool), typeof(XListBoxItem), new FrameworkPropertyMetadata(false));
        public static bool GetLastSelected(ListBoxItem i) => (bool)i.GetValue(LastSelectedProperty);
        public static void SetLastSelected(ListBoxItem i, bool value) => i.SetValue(LastSelectedProperty, value);

        #endregion
    }

    [Extends(typeof(ListViewItem))]
    public static class XListViewItem
    {
        #region ParentHasColumns

        public static readonly DependencyProperty ParentHasColumnsProperty = DependencyProperty.RegisterAttached("ParentHasColumns", typeof(bool), typeof(XListViewItem), new FrameworkPropertyMetadata(false));
        public static bool GetParentHasColumns(ListViewItem i) => (bool)i.GetValue(ParentHasColumnsProperty);
        public static void SetParentHasColumns(ListViewItem i, bool value) => i.SetValue(ParentHasColumnsProperty, value);

        #endregion
    }
}