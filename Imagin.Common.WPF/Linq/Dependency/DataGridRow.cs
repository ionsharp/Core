using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(DataGridRow))]
    public static class XDataGridRow
    {
        public static readonly ResourceKey<DataGridRow> TemplateKey = new();

        public static readonly ResourceKey ValidationErrorTemplateKey = new();

        #region DoubleClickCommand

        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(XDataGridRow), new FrameworkPropertyMetadata(null, OnDoubleClickCommandChanged));
        public static ICommand GetDoubleClickCommand(DataGridRow i) => (ICommand)i.GetValue(DoubleClickCommandProperty);
        public static void SetDoubleClickCommand(DataGridRow i, ICommand input) => i.SetValue(DoubleClickCommandProperty, input);
        static void OnDoubleClickCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGridRow row)
                row.RegisterHandlerAttached(e.NewValue != null, DoubleClickCommandProperty, i => i.MouseDoubleClick += DoubleClickCommand_MouseDoubleClick, i => i.MouseDoubleClick -= DoubleClickCommand_MouseDoubleClick);
        }

        static void DoubleClickCommand_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (sender is DataGridRow row)
                GetDoubleClickCommand(row).Execute(GetDoubleClickCommandParameter(row));
        }

        #endregion

        #region DoubleClickCommandParameter

        public static readonly DependencyProperty DoubleClickCommandParameterProperty = DependencyProperty.RegisterAttached("DoubleClickCommandParameter", typeof(object), typeof(XDataGridRow), new FrameworkPropertyMetadata(null));
        public static object GetDoubleClickCommandParameter(DataGridRow i) => i.GetValue(DoubleClickCommandParameterProperty);
        public static void SetDoubleClickCommandParameter(DataGridRow i, object input) => i.SetValue(DoubleClickCommandParameterProperty, input);

        #endregion

        #region SelectionVisibility

        public static readonly DependencyProperty SelectionVisibilityProperty = DependencyProperty.RegisterAttached("SelectionVisibility", typeof(Visibility), typeof(XDataGridRow), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public static Visibility GetSelectionVisibility(DataGridRow i) => (Visibility)i.GetValue(SelectionVisibilityProperty);
        public static void SetSelectionVisibility(DataGridRow i, Visibility value) => i.SetValue(SelectionVisibilityProperty, value);

        #endregion

        #region Methods

        public static List<DataGridCell> Cells(this DataGridRow input)
        {
            var result = new List<DataGridCell>();
            if (input.FindVisualChildOfType<DataGridCellsPresenter>() is DataGridCellsPresenter presenter)
            {
                foreach (var i in presenter.Items)
                {
                    if (presenter.ItemContainerGenerator.ContainerFromItem(i) is DataGridCell j)
                        result.Add(j);
                }
            }
            return result;
        }

        #endregion
    }
}