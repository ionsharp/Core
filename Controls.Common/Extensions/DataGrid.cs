using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataGridExtensions
    {
        #region ExtendsDefaultBehavior

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ExtendsDefaultBehaviorProperty = DependencyProperty.RegisterAttached("ExtendsDefaultBehavior", typeof(bool), typeof(DataGridExtensions), new PropertyMetadata(false, OnExtendsDefaultBehaviorChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetExtendsDefaultBehavior(DataGrid d)
        {
            return (bool)d.GetValue(ExtendsDefaultBehaviorProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetExtendsDefaultBehavior(DataGrid d, bool value)
        {
            d.SetValue(ExtendsDefaultBehaviorProperty, value);
        }
        static void OnExtendsDefaultBehaviorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid)
            {
                var DataGrid = sender as DataGrid;
                if ((bool)e.NewValue)
                    DataGrid.CommandBindings.Add(new CommandBinding(AddCommand, OnAddCommandExecuted, OnAddCommandCanExecute));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedUICommand AddCommand = new RoutedUICommand("AddCommand", "AddCommand", typeof(DataGridExtensions));
        static void OnAddCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var d = sender as DataGrid;

            var ItemType = default(Type);

            var Arguments = d.ItemsSource.GetType().GetGenericArguments();
            if (Arguments.Length == 1)
            {
                ItemType = Arguments.Single();
            }
            else if (d.Items.Count > 0)
            {
                ItemType = d.Items[0].GetType();
            }
            else
            {
                Arguments = d.ItemsSource.GetType().BaseType.GetGenericArguments();
                if (Arguments.Length == 1)
                    ItemType = Arguments.Single();
            }

            if (ItemType != default(Type) && d.ItemsSource is IList)
                d.ItemsSource.As<IList>()?.Add(Activator.CreateInstance(ItemType));
        }
        static void OnAddCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as DataGrid).CanUserAddRows;
        }

        #endregion

        #region DisplayRowNumber

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DisplayRowNumberProperty = DependencyProperty.RegisterAttached("DisplayRowNumber", typeof(bool), typeof(DataGridExtensions), new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetDisplayRowNumber(DataGrid d)
        {
            return (bool)d.GetValue(DisplayRowNumberProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetDisplayRowNumber(DataGrid d, bool value)
        {
            d.SetValue(DisplayRowNumberProperty, value);
        }
        static void OnDisplayRowNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var DataGrid = d as DataGrid;

            DataGrid.LoadingRow -= OnLoadingRow;
            DataGrid.UnloadingRow -= OnLoadingRow;

            if ((bool)e.NewValue)
            {
                DataGrid.LoadingRow += OnLoadingRow;
                DataGrid.UnloadingRow += OnLoadingRow;

                if (DataGrid.IsLoaded)
                    DataGrid.GetVisualChildren<DataGridRow>().ForEach(i => i.Header = i.GetIndex() + GetDisplayRowNumberOffset(DataGrid));
            }
            else DataGrid.GetVisualChildren<DataGridRow>().ToList<DataGridRow>().ForEach(i => i.Header = string.Empty);
        }

        static void OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var d = sender as DataGrid;
            e.Row.Header = e.Row.GetIndex() + GetDisplayRowNumberOffset(d);
        }

        #endregion

        #region DisplayRowNumberOffset

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DisplayRowNumberOffsetProperty = DependencyProperty.RegisterAttached("DisplayRowNumberOffset", typeof(int), typeof(DataGridExtensions), new FrameworkPropertyMetadata(0, OnDisplayRowNumberOffsetChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int GetDisplayRowNumberOffset(DependencyObject d)
        {
            return (int)d.GetValue(DisplayRowNumberOffsetProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetDisplayRowNumberOffset(DependencyObject d, int value)
        {
            d.SetValue(DisplayRowNumberOffsetProperty, value);
        }
        static void OnDisplayRowNumberOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var DataGrid = d as DataGrid;
            var Offset = (int)e.NewValue;

            if (GetDisplayRowNumber(DataGrid))
                DataGrid.GetVisualChildren<DataGridRow>().ForEach(i => i.Header = i.GetIndex() + Offset);
        }

        #endregion

        #region ScrollAddedIntoView

        /// <summary>
        /// Determines whether or not to scroll newly added items into view.
        /// </summary>
        public static readonly DependencyProperty ScrollAddedIntoViewProperty = DependencyProperty.RegisterAttached("ScrollAddedIntoView", typeof(bool), typeof(DataGridExtensions), new PropertyMetadata(false, OnScrollAddedIntoViewChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetScrollAddedIntoView(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollAddedIntoViewProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetScrollAddedIntoView(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollAddedIntoViewProperty, value);
        }
        static void OnScrollAddedIntoViewChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var DataGrid = sender as DataGrid;

            if ((bool)e.NewValue)
            {
                DataGrid.InitializingNewItem += OnScrollAddedIntoViewChanged;
            }
            else DataGrid.InitializingNewItem -= OnScrollAddedIntoViewChanged;
        }
        static void OnScrollAddedIntoViewChanged(object sender, InitializingNewItemEventArgs e)
        {
            var DataGrid = sender as DataGrid;
            DataGrid.ScrollIntoView(e.NewItem);
        }

        #endregion
    }
}