using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(ListBox))]
    public static class XListBox
    {
        #region LastSelected

        internal static readonly DependencyProperty LastSelectedProperty = DependencyProperty.RegisterAttached("LastSelected", typeof(ListBoxItem), typeof(XListBox), new FrameworkPropertyMetadata(null));
        internal static ListBoxItem GetLastSelected(ListBox i) => (ListBoxItem)i.GetValue(LastSelectedProperty);
        internal static void SetLastSelected(ListBox i, ListBoxItem value) => i.SetValue(LastSelectedProperty, value);

        public static readonly DependencyProperty EnableLastSelectedProperty = DependencyProperty.RegisterAttached("EnableLastSelected", typeof(bool), typeof(XListBox), new FrameworkPropertyMetadata(false, OnEnableLastSelectedChanged));
        public static bool GetEnableLastSelected(ListBox i) => (bool)i.GetValue(EnableLastSelectedProperty);
        public static void SetEnableLastSelected(ListBox i, bool value) => i.SetValue(EnableLastSelectedProperty, value);
        static void OnEnableLastSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ListBox listBox)
                listBox.RegisterHandlerAttached((bool)e.NewValue, EnableLastSelectedProperty, i => i.SelectionChanged += EnableLastSelected_SelectionChanged, i => i.SelectionChanged -= EnableLastSelected_SelectionChanged);
        }

        static void EnableLastSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox.SelectedItems.Count > 0)
            {
                foreach (var i in listBox.Items)
                {
                    if (listBox.ItemContainerGenerator.ContainerFromItem(i) is ListBoxItem j)
                        XListBoxItem.SetLastSelected(j, false);
                }

                var last = e.AddedItems?.Last();
                if (listBox.ItemContainerGenerator.ContainerFromItem(last) is ListBoxItem k)
                    SetLastSelected(listBox, k);
            }
            else
            {
                var lastSelected = GetLastSelected(listBox);
                foreach (var i in listBox.Items)
                {
                    if (listBox.ItemContainerGenerator.ContainerFromItem(i) is ListBoxItem j)
                        XListBoxItem.SetLastSelected(j, j.Equals(lastSelected));
                }
            }
        }

        #endregion
    }
}