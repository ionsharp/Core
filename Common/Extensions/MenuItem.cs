using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Extensions
{
    public class MenuItemExtensions : DependencyObject
    {
        #region Private

        static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (ElementToGroupNames == null)
                ElementToGroupNames = new Dictionary<MenuItem, string>();

            //Add an entry to the group name collection
            var Item = d as MenuItem;
            if (Item == null)
                return;

            if (Item.IsChecked)
                Item.IsEnabled = false;

            string NewName = e.NewValue.ToString();
            string OldName = e.OldValue.ToString();
            //Removing the toggle button from grouping
            if (string.IsNullOrEmpty(NewName))
                RemoveCheckboxFromGrouping(Item);
            //Switching to a new group
            else if (NewName != OldName)
            {
                //Remove the old group mapping
                if (!string.IsNullOrEmpty(OldName))
                    RemoveCheckboxFromGrouping(Item);
                ElementToGroupNames.Add(Item, e.NewValue.ToString());
                Item.Checked += OnMenuItemChecked;
            }
        }

        static void OnMenuItemChecked(object sender, RoutedEventArgs e)
        {
            var MenuItem = e.OriginalSource as MenuItem;
            MenuItem.IsEnabled = false;
            foreach (var i in ElementToGroupNames)
            {
                if (i.Key != MenuItem && i.Value == GetGroupName(MenuItem))
                {
                    i.Key.IsChecked = false;
                    i.Key.IsEnabled = true;
                }
            }
        }

        static void RemoveCheckboxFromGrouping(MenuItem MenuItem)
        {
            if (ElementToGroupNames == null)
                ElementToGroupNames = new Dictionary<MenuItem, string>();
            ElementToGroupNames.Remove(MenuItem);
            MenuItem.Checked -= OnMenuItemChecked;
        }

        #endregion

        #region Public

        public static Dictionary<MenuItem, string> ElementToGroupNames;

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached("GroupName", typeof(System.String), typeof(MenuItemExtensions), new PropertyMetadata(System.String.Empty, OnGroupNameChanged));
        public static void SetGroupName(MenuItem MenuItem, string Value)
        {
            MenuItem.SetValue(GroupNameProperty, Value);
        }
        public static string GetGroupName(MenuItem MenuItem)
        {
            return MenuItem.GetValue(GroupNameProperty).ToString();
        }

        #endregion
    }
}