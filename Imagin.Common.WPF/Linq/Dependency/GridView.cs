using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(GridView))]
    public static class XGridView
    {
        #region (internal) LastClicked

        internal static readonly DependencyProperty LastClickedProperty = DependencyProperty.RegisterAttached("LastClicked", typeof(GridViewColumnHeader), typeof(XGridView), new FrameworkPropertyMetadata(null));
        internal static GridViewColumnHeader GetLastClicked(GridView i) => (GridViewColumnHeader)i.GetValue(LastClickedProperty);
        internal static void SetLastClicked(GridView i, GridViewColumnHeader input) => i.SetValue(LastClickedProperty, input);

        #endregion
    }
}