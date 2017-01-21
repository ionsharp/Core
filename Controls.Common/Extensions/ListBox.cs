using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common.Extensions
{
    public static class ListBoxExtensions
    {
        #region IsDirectionalSelectionEnabled

        /// <summary>
        /// Determines whether or not directional keys can 
        /// be used to select items; intended for complex 
        /// views where items are organized in both rows 
        /// and columns.
        /// </summary>
        /// <pseudo>
        /// If up or left is clicked and nothing is selected, 
        /// select first. If bottom or right is clicked and 
        /// nothing is selected, select last. If first is 
        /// selected and clicking left or up, select last. 
        /// If last is selected and clicking right or down, 
        /// select first.
        /// </pseudo>
        public static readonly DependencyProperty IsDirectionalSelectionEnabledProperty = DependencyProperty.RegisterAttached("IsDirectionalSelectionEnabled", typeof(bool), typeof(ListBoxExtensions), new PropertyMetadata(false, OnIsDirectionalSelectionEnabled));
        public static bool GetIsDirectionalSelectionEnabled(ListBox d)
        {
            return (bool)d.GetValue(IsDirectionalSelectionEnabledProperty);
        }
        public static void SetIsDirectionalSelectionEnabled(ListBox d, bool value)
        {
            d.SetValue(IsDirectionalSelectionEnabledProperty, value);
        }
        static void OnIsDirectionalSelectionEnabled(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ListBox = sender as ListBox;
            if (ListBox != null)
            {
                if ((bool)e.NewValue)
                {
                    ListBox.IsSynchronizedWithCurrentItem = true;
                    ListBox.PreviewKeyDown += OnPreviewKeyDown;
                }
                else
                {
                    ListBox.IsSynchronizedWithCurrentItem = false;

                }
            }
        }

        static void MoveLeft(ListBox ListBox)
        {
            //If nothing is selected, first item is selected.
            if (ListBox.SelectedItems.Count == 0)
                ListBox.Items.MoveCurrentToFirst();
            else if (!ListBox.Items.MoveCurrentToPrevious())
            {
                //First is already selected; if wrapped, select last.
                if (GetSelectionWrap(ListBox))
                    ListBox.Items.MoveCurrentToLast();
            }
        }

        static void MoveRight(ListBox ListBox)
        {
            //If nothing is selected, last item is selected.
            if (ListBox.SelectedItems.Count == 0)
                ListBox.Items.MoveCurrentToLast();
            else if (!ListBox.Items.MoveCurrentToNext())
            {
                //Last is already selected; if wrapped, select first.
                if (GetSelectionWrap(ListBox))
                    ListBox.Items.MoveCurrentToFirst();
            }
        }

        static void MoveNext(ListBox ListBox, Key Key, int ItemsPerRow)
        {
            int? Index = null;

            var Count = ListBox.Items.Count;
            if (Key == Key.Up)
            {
                Index = ListBox.SelectedIndex - ItemsPerRow;
                if (Index < 0)
                    Index = GetSelectionWrap(ListBox) ? Count + Index : null;
            }
            else if (Key == Key.Down)
            {
                Index = ListBox.SelectedIndex + ItemsPerRow;
                if (Index >= Count)
                    Index = GetSelectionWrap(ListBox) ? Index - Count : null;
            }
            if (Index != null)
                ListBox.SelectedIndex = Index < 0 ? 0 : (Index >= Count ? Count - 1 : Index.Value);
        }

        static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var ListBox = sender as ListBox;
            if (ListBox.Items.Count != 0) return;

            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                    var Width = ListBox.ActualWidth - ListBox.Padding.Left - ListBox.Padding.Right;

                    var Item = (ListBox.ItemContainerGenerator.ContainerFromItem(ListBox.Items.CurrentItem) as ListBoxItem);
                    var ItemWidth = Item.ActualWidth + Item.Margin.Left + Item.Margin.Right;
                    int ItemsPerRow = (Width / ItemWidth).Round().ToInt32();

                    //If there is only one item per row, up corresponds to left and  down corresponds to right.
                    if (ItemsPerRow <= 1)
                    {
                        if (e.Key == Key.Up)
                            MoveLeft(ListBox);
                        else if (e.Key == Key.Down)
                            MoveRight(ListBox);
                    }
                    else MoveNext(ListBox, e.Key, ItemsPerRow);
                    break;
                case Key.Left:
                    MoveLeft(ListBox);
                    break;
                case Key.Right:
                    MoveRight(ListBox);
                    break;
            }
        }

        #endregion

        #region SelectionWrap

        /// <summary>
        /// Determines whether or not selections made with directional keys "wrap" ends.
        /// </summary>
        /// <remarks>
        /// IsDirectionalSelectionEnabled must be set to true.
        /// </remarks>
        public static readonly DependencyProperty SelectionWrapProperty = DependencyProperty.RegisterAttached("SelectionWrap", typeof(bool), typeof(ListBoxExtensions), new PropertyMetadata(false, null, new CoerceValueCallback(OnSelectionWrapCoerced)));
        public static bool GetSelectionWrap(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectionWrapProperty);
        }
        public static void SetSelectionWrap(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectionWrapProperty, value);
        }
        static object OnSelectionWrapCoerced(DependencyObject d, object value)
        {
            var ListBox = d as ListBox;
            if ((bool)value && !GetIsDirectionalSelectionEnabled(ListBox))
                return false;
            return value;
        }

        #endregion
    }
}
