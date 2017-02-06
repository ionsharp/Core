using Imagin.Common.Collections;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Stores reference to every <see cref="DataGrid"/> with <see cref="DataGridExtensions.ScrollAddedIntoViewProperty"/> enabled; the key is the hash code of the value's underlying collection.
        /// </summary>
        static Dictionary<int, DataGrid> _ScrollAddedIntoView = new Dictionary<int, DataGrid>();

        /// <summary>
        /// Gets or sets whether or not to scroll newly added items into view; note, the underlying collection MUST implement <see cref="ITrackableCollection{T}"/>.
        /// </summary>
        public static readonly DependencyProperty ScrollAddedIntoViewProperty = DependencyProperty.RegisterAttached("ScrollAddedIntoView", typeof(bool), typeof(DataGridExtensions), new PropertyMetadata(false, OnScrollAddedIntoViewChanged));
        /// <summary>
        /// 
        /// </summary>
        public static bool GetScrollAddedIntoView(DataGrid d)
        {
            return (bool)d.GetValue(ScrollAddedIntoViewProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetScrollAddedIntoView(DataGrid d, bool value)
        {
            d.SetValue(ScrollAddedIntoViewProperty, value);
        }

        /// <summary>
        /// Occurs when <see cref="DataGridExtensions.ScrollAddedIntoViewProperty"/> changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnScrollAddedIntoViewChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var d = sender.As<DataGrid>();
            d.Loaded -= OnScrollAddedIntoViewChanged;

            var s = d?.ItemsSource;
            var i = s as ITrackableCollection;
            var h = i?.GetHashCode();

            if (i != null)
            {
                OnScrollAddedIntoViewChanged(d, i, (bool)e.NewValue);
            }
            //If collection hasn't otherwise been assigned yet, attempt to register property when host loads.
            else d.Loaded += OnScrollAddedIntoViewChanged;
        }

        /// <summary>
        /// Adds/removes and registers/unregisters events associated with <see cref="DataGridExtensions.ScrollAddedIntoViewProperty"/>, respectively.
        /// </summary>
        /// <param name="DataGrid"></param>
        /// <param name="Collection"></param>
        /// <param name="Register"></param>
        static void OnScrollAddedIntoViewChanged(DataGrid DataGrid, ITrackableCollection Collection, bool Register)
        {
            var h = Collection.GetHashCode();
            if (Register)
            {
                _ScrollAddedIntoView.Add(h, DataGrid);
                Collection.ItemAdded += OnScrollAddedIntoViewChanged;
            }
            else
            {
                Collection.ItemAdded -= OnScrollAddedIntoViewChanged;
                _ScrollAddedIntoView.Remove(h);
            }
        }

        /// <summary>
        /// Occurs when <see cref="DataGrid"/> loads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnScrollAddedIntoViewChanged(object sender, RoutedEventArgs e)
        {
            var d = sender.As<DataGrid>();
            var s = d?.ItemsSource;
            var i = s as ITrackableCollection;
            var h = i?.GetHashCode();

            if (i != null)
            {
                //Once we have access to collection, avoid for each future load.
                d.Loaded -= OnScrollAddedIntoViewChanged;
                OnScrollAddedIntoViewChanged(d, i, GetScrollAddedIntoView(d));
            }
        }

        /// <summary>
        /// Occurs when an item is added to the <see cref="DataGrid"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnScrollAddedIntoViewChanged(object sender, EventArgs<object> e)
        {
            _ScrollAddedIntoView[sender.As<ITrackableCollection>().GetHashCode()]?.As<DataGrid>()?.ScrollIntoView(e.Value);
        }

        #endregion
    }
}