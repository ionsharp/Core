using Imagin.Common.Extensions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common.Extensions
{
    public enum MenuItemSelectionMode
    {
        /// <summary>
        /// One item may be checked.
        /// </summary>
        Single,
        /// <summary>
        /// Either one or no items may be checked.
        /// </summary>
        SingleOrNone,
        /// <summary>
        /// Any number of items may be checked; equivalent to
        /// not specifying a group name.
        /// </summary>
        Multiple
    }

    public static class MenuItemExtensions
    {
        #region Private

        /// <remarks>
        /// Let's assume the MenuItem was generated from
        /// a collection of enum values. The data context 
        /// for the MenuItem would be = to a unique enum 
        /// value. The GroupSource should be bound to a 
        /// property somewhere that stores a reference to 
        /// the current enum value. When checking a MenuItem,
        /// it is necessary to update the GroupSource so the
        /// source reflects the checked value. The GroupSource
        /// should only update when an initial value has
        /// been set. 
        /// </remarks>
        static void OnChecked(object sender, RoutedEventArgs e)
        {
            var MenuItem = e.OriginalSource as MenuItem;

            var SelectionMode = GetSelectionMode(MenuItem);
            switch (SelectionMode)
            {
                case MenuItemSelectionMode.Single:
                    MenuItem.IsEnabled = false;
                    break;
                case MenuItemSelectionMode.Multiple:
                case MenuItemSelectionMode.SingleOrNone:
                    break;
            }

            var Parent = MenuItem.GetVisualParent<MenuItem>();
            if (Parent != null)
                MenuItemExtensions.SetGroupSource(Parent, MenuItem.DataContext);

            if (SelectionMode == MenuItemSelectionMode.Single || SelectionMode == MenuItemSelectionMode.SingleOrNone)
            {
                foreach (var i in ElementToGroupNames)
                {
                    if (i.Key != MenuItem && i.Value == GetGroupName(MenuItem))
                    {
                        i.Key.IsChecked = false;
                        if (SelectionMode == MenuItemSelectionMode.Single)
                            i.Key.IsEnabled = true;
                    }
                }
            }
        }

        static void RemoveFromGrouping(MenuItem MenuItem)
        {
            if (ElementToGroupNames == null)
                ElementToGroupNames = new Dictionary<MenuItem, string>();
            ElementToGroupNames.Remove(MenuItem);
            MenuItem.Checked -= OnChecked;
        }

        #endregion

        #region Public

        public static Dictionary<MenuItem, string> ElementToGroupNames;

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached("GroupName", typeof(string), typeof(MenuItemExtensions), new PropertyMetadata(string.Empty, OnGroupNameChanged));
        public static void SetGroupName(MenuItem MenuItem, string Value)
        {
            MenuItem.SetValue(GroupNameProperty, Value);
        }
        public static string GetGroupName(MenuItem MenuItem)
        {
            return MenuItem.GetValue(GroupNameProperty).ToString();
        }
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

            var NewName = e.NewValue.ToString();
            var OldName = e.OldValue.ToString();

            //Removing the toggle button from grouping
            if (NewName.IsNullOrEmpty())
                RemoveFromGrouping(Item);

            //Switching to a new group
            else if (NewName != OldName)
            {
                //Remove the old group mapping
                if (!OldName.IsNullOrEmpty())
                    RemoveFromGrouping(Item);

                ElementToGroupNames.Add(Item, e.NewValue.ToString());
                Item.Checked += OnChecked;
            }
        }

        public static readonly DependencyProperty GroupSourceProperty = DependencyProperty.RegisterAttached("GroupSource", typeof(object), typeof(MenuItemExtensions), new PropertyMetadata(null));
        public static void SetGroupSource(MenuItem MenuItem, object Value)
        {
            MenuItem.SetValue(GroupSourceProperty, Value);
        }
        public static object GetGroupSource(MenuItem MenuItem)
        {
            return MenuItem.GetValue(GroupSourceProperty);
        }

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.RegisterAttached("SelectionMode", typeof(MenuItemSelectionMode), typeof(MenuItemExtensions), new PropertyMetadata(MenuItemSelectionMode.Single));
        public static void SetSelectionMode(MenuItem MenuItem, MenuItemSelectionMode Value)
        {
            MenuItem.SetValue(SelectionModeProperty, Value);
        }
        public static MenuItemSelectionMode GetSelectionMode(MenuItem MenuItem)
        {
            return (MenuItemSelectionMode)MenuItem.GetValue(SelectionModeProperty);
        }

        #endregion
    }
}
