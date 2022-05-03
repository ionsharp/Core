using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(GridViewColumnHeader))]
    public static class XGridViewColumnHeader
    {
        #region Properties

        #region (private) LastDirection

        static readonly DependencyProperty LastDirectionProperty = DependencyProperty.RegisterAttached("LastDirection", typeof(ListSortDirection), typeof(XGridViewColumnHeader), new FrameworkPropertyMetadata(default(ListSortDirection)));
        static ListSortDirection GetLastDirection(GridViewColumnHeader i) => (ListSortDirection)i.GetValue(LastDirectionProperty);
        static void SetLastDirection(GridViewColumnHeader i, ListSortDirection input) => i.SetValue(LastDirectionProperty, input);

        #endregion

        #region (private) Parent

        static readonly DependencyProperty ParentProperty = DependencyProperty.RegisterAttached("Parent", typeof(GridView), typeof(XGridViewColumnHeader), new FrameworkPropertyMetadata(null));
        static GridView GetParent(GridViewColumnHeader i) => (GridView)i.GetValue(ParentProperty);
        static void SetParent(GridViewColumnHeader i, GridView input) => i.SetValue(ParentProperty, input);

        #endregion

        #region SortDirection

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.RegisterAttached("SortDirection", typeof(ListSortDirection?), typeof(XGridViewColumnHeader), new FrameworkPropertyMetadata(null));
        public static ListSortDirection? GetSortDirection(GridViewColumnHeader i) => (ListSortDirection?)i.GetValue(SortDirectionProperty);
        static void SetSortDirection(GridViewColumnHeader i, ListSortDirection? input) => i.SetValue(SortDirectionProperty, input);

        #endregion

        #endregion

        #region XGridViewColumnHeader

        static XGridViewColumnHeader()
        {
            EventManager.RegisterClassHandler(typeof(GridViewColumnHeader), GridViewColumnHeader.ClickEvent,
                new RoutedEventHandler(OnClick), true);
        }

        static void OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader header)
            {
                if (GetParent(header) == null)
                    SetParent(header, header.FindParent<GridView>());

                var parent = GetParent(header);
                if (header?.Column != null)
                {
                    var direction = ListSortDirection.Ascending;
                    if (header.Role != GridViewColumnHeaderRole.Padding)
                    {
                        if (header != XGridView.GetLastClicked(parent))
                        {
                            direction = ListSortDirection.Ascending;
                        }
                        else direction
                            = GetLastDirection(header) == ListSortDirection.Ascending
                            ? ListSortDirection.Descending
                            : ListSortDirection.Ascending;
                    }

                    var sortName = header.Column.Header as string;
                    if (header.FindParent<ListView>() is ListView listView)
                    {
                        XItemsControl
                            .SetSortDirection(listView, direction);
                        XItemsControl
                            .SetSortName(listView, sortName);
                    }

                    SetSortDirection(header, direction);

                    // Remove arrow from previously sorted header
                    if (XGridView.GetLastClicked(parent) != null && XGridView.GetLastClicked(parent) != header)
                        SetSortDirection(XGridView.GetLastClicked(parent), null);

                    XGridView.SetLastClicked(parent, header);
                    SetLastDirection(header, direction);
                }
            }
        }

        #endregion
    }
}