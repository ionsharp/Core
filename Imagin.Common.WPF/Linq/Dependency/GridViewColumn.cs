using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(GridViewColumn))]
    public static class XGridViewColumn
    {
        #region (internal) ColumnIndex

        internal static readonly DependencyProperty ColumnIndexProperty = DependencyProperty.RegisterAttached("ColumnIndex", typeof(int), typeof(XGridViewColumn), new FrameworkPropertyMetadata(-1));
        internal static int GetColumnIndex(GridViewColumn i) => (int)i.GetValue(ColumnIndexProperty);
        internal static void SetColumnIndex(GridViewColumn i, int input) => i.SetValue(ColumnIndexProperty, input);

        #endregion
    }
}