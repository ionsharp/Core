using Imagin.Common.Extensions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Imagin.Controls.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataGridExtensions
    {
        #region DisplayRowNumber

        public static DependencyProperty DisplayRowNumberProperty = DependencyProperty.RegisterAttached("DisplayRowNumber", typeof(bool), typeof(DataGridExtensions), new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged));
        public static bool GetDisplayRowNumber(DataGrid d)
        {
            return (bool)d.GetValue(DisplayRowNumberProperty);
        }
        public static void SetDisplayRowNumber(DataGrid d, bool value)
        {
            d.SetValue(DisplayRowNumberProperty, value);
        }
        static void OnDisplayRowNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var DataGrid = d as DataGrid;
            if ((bool)e.NewValue == true)
            {
                EventHandler<DataGridRowEventArgs> LoadedRowHandler = null;
                LoadedRowHandler = (object a, DataGridRowEventArgs b) =>
                {
                    if (GetDisplayRowNumber(DataGrid) == false)
                    {
                        DataGrid.LoadingRow -= LoadedRowHandler;
                        return;
                    }
                    b.Row.Header = b.Row.GetIndex() + GetDisplayRowNumberOffset(d);
                };
                DataGrid.LoadingRow += LoadedRowHandler;

                ItemsChangedEventHandler ItemsChangedHandler = null;
                ItemsChangedHandler = (object a, ItemsChangedEventArgs b) =>
                {
                    if (GetDisplayRowNumber(DataGrid) == false)
                    {
                        DataGrid.ItemContainerGenerator.ItemsChanged -= ItemsChangedHandler;
                        return;
                    }
                    DataGrid.GetVisualChildren<DataGridRow>().ToList<DataGridRow>().ForEach(f => f.Header = f.GetIndex() + GetDisplayRowNumberOffset(DataGrid));
                };
                DataGrid.ItemContainerGenerator.ItemsChanged += ItemsChangedHandler;
            }
            else
            {

            }
        }

        #endregion

        #region DisplayRowNumberOffset

        public static DependencyProperty DisplayRowNumberOffsetProperty = DependencyProperty.RegisterAttached("DisplayRowNumberOffset", typeof(int), typeof(DataGridExtensions), new FrameworkPropertyMetadata(0, OnDisplayRowNumberOffsetChanged));
        public static int GetDisplayRowNumberOffset(DependencyObject d)
        {
            return (int)d.GetValue(DisplayRowNumberOffsetProperty);
        }
        public static void SetDisplayRowNumberOffset(DependencyObject d, int value)
        {
            d.SetValue(DisplayRowNumberOffsetProperty, value);
        }
        static void OnDisplayRowNumberOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var DataGrid = d as DataGrid;
            var Offset = (int)e.NewValue;

            if (GetDisplayRowNumber(DataGrid))
                DataGrid.GetVisualChildren<DataGridRow>().ToList<DataGridRow>().ForEach(i => i.Header = i.GetIndex() + Offset);
        }

        #endregion

        #region RegisterAddCommand

        public static readonly DependencyProperty RegisterAddCommandProperty = DependencyProperty.RegisterAttached("RegisterAddCommand", typeof(bool), typeof(DataGridExtensions), new PropertyMetadata(false, OnRegisterAddCommandChanged));
        public static bool GetRegisterAddCommand(DependencyObject obj)
        {
            return (bool)obj.GetValue(RegisterAddCommandProperty);
        }
        public static void SetRegisterAddCommand(DependencyObject obj, bool value)
        {
            obj.SetValue(RegisterAddCommandProperty, value);
        }
        static void OnRegisterAddCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid)
            {
                var DataGrid = sender as DataGrid;
                if ((bool)e.NewValue)
                    DataGrid.CommandBindings.Add(new CommandBinding(AddCommand, AddCommand_Executed, AddCommand_CanExecute));
            }
        }

        public static readonly RoutedUICommand AddCommand = new RoutedUICommand("AddCommand", "AddCommand", typeof(DataGridExtensions));
        static void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var DataGrid = sender as DataGrid;

            var ItemType = default(Type);

            var GenericArguments = DataGrid.ItemsSource.GetType().GetGenericArguments();

            //If contains one generic argument, get type from that.
            if (GenericArguments.Length == 1)
                ItemType = GenericArguments.Single();
            //Else, if contains at least one item, get type from that item.
            else if (DataGrid.Items.Count > 0)
                ItemType = DataGrid.Items[0].GetType();
            else
            {
                var BaseGenericArguments = DataGrid.ItemsSource.GetType().BaseType.GetGenericArguments();
                //Else, check if base type has one generic argument and get type from that.
                if (BaseGenericArguments.Length == 1)
                    ItemType = BaseGenericArguments.Single();
                //Else, give up
            }

            if (ItemType != default(Type))
                DataGrid.Items.Add(Activator.CreateInstance(ItemType));
        }
        static void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as DataGrid).CanUserAddRows;
        }

        #endregion

        #region ScrollAddedIntoView

        /// <summary>
        /// Determines whether or not to scroll newly added items into view.
        /// </summary>
        public static readonly DependencyProperty ScrollAddedIntoViewProperty = DependencyProperty.RegisterAttached("ScrollAddedIntoView", typeof(bool), typeof(DataGridExtensions), new PropertyMetadata(false, OnScrollAddedIntoViewChanged));
        public static bool GetScrollAddedIntoView(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollAddedIntoViewProperty);
        }
        public static void SetScrollAddedIntoView(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollAddedIntoViewProperty, value);
        }
        static void OnScrollAddedIntoViewChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion
    }
}