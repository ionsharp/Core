using Imagin.Common.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MenuItemExtensions
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<MenuItem, string> elements = new Dictionary<MenuItem, string>();

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached("GroupName", typeof(string), typeof(MenuItemExtensions), new PropertyMetadata(string.Empty, OnGroupNameChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="Value"></param>
        public static void SetGroupName(MenuItem d, string Value)
        {
            d.SetValue(GroupNameProperty, Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string GetGroupName(MenuItem d)
        {
            return d.GetValue(GroupNameProperty).ToString();
        }
        static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var MenuItem = d as MenuItem;

            if (MenuItem != null)
            {
                var NewName = e.NewValue.ToString();
                var OldName = e.OldValue.ToString();

                //Switching to no group
                if (NewName.IsNullOrEmpty())
                {
                    RemoveGrouping(MenuItem);
                }
                //Switching to new group
                else if (NewName != OldName)
                {
                    if (!OldName.IsNullOrEmpty())
                        RemoveGrouping(MenuItem);

                    AddGrouping(MenuItem, e.NewValue.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.RegisterAttached("SelectionMode", typeof(MenuItemSelectionMode), typeof(MenuItemExtensions), new PropertyMetadata(MenuItemSelectionMode.Single, OnSelectionModeChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="Value"></param>
        public static void SetSelectionMode(MenuItem d, MenuItemSelectionMode Value)
        {
            d.SetValue(SelectionModeProperty, Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static MenuItemSelectionMode GetSelectionMode(MenuItem d)
        {
            return (MenuItemSelectionMode)d.GetValue(SelectionModeProperty);
        }
        static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var MenuItem = d as MenuItem;

            //If going from multiple to single, 
            if (e.OldValue?.To<MenuItemSelectionMode>() == MenuItemSelectionMode.Multiple)
            {
                var GroupName = GetGroupName(MenuItem);

                var j = false;
                foreach (var i in elements)
                {
                    if (i.Value == GroupName)
                    {
                        //If we haven't checked anything yet and the current item is checked, check it; else, uncheck
                        i.Key.IsChecked = !j && i.Key.IsChecked;
                        //If the item was checked, we have checked the first item
                        j = i.Key.IsChecked;
                    }
                }
            }
        }

        #endregion

        #region Methods

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
            //If multiple elements in the same group cannot be checked at the same time
            if (SelectionMode != MenuItemSelectionMode.Multiple)
            {
                //Uncheck all elements except current
                foreach (var i in elements)
                {
                    if (i.Key != MenuItem && i.Value == GetGroupName(MenuItem))
                        i.Key.IsChecked = false;
                }
            }
        }

        static void OnClick(object sender, RoutedEventArgs e)
        {
            var MenuItem = sender as MenuItem;
            //Make sure it's still checkable! (see OnPreviewMouseDown)
            if (MenuItem.IsChecked && GetSelectionMode(MenuItem) == MenuItemSelectionMode.Single)
                MenuItem.IsCheckable = true;
        }

        static void OnPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var MenuItem = sender as MenuItem;
            //If it's already checked, we're about to uncheck it; if one element must always be checked, disable unchecking
            if (MenuItem.IsChecked && GetSelectionMode(MenuItem) == MenuItemSelectionMode.Single)
                MenuItem.IsCheckable = false;
        }

        static void AddGrouping(MenuItem MenuItem, string GroupName)
        {
            MenuItem.Checked += OnChecked;
            MenuItem.Click += OnClick;
            MenuItem.PreviewMouseDown += OnPreviewMouseDown;
            elements.Add(MenuItem, GroupName);
        }

        static void RemoveGrouping(MenuItem MenuItem)
        {
            MenuItem.Checked -= OnChecked;
            MenuItem.Click -= OnClick;
            MenuItem.PreviewMouseDown -= OnPreviewMouseDown;
            elements.Remove(MenuItem);
        }

        #endregion
    }
}
