using Imagin.Common.Collections;
using Imagin.Common.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Linq
{
    [Extends(typeof(Selector))]
    public static class XSelector
    {
        #region SelectedItems

        static readonly DependencyPropertyKey SelectedItemsKey = DependencyProperty.RegisterAttachedReadOnly("SelectedItems", typeof(ICollectionChanged), typeof(XSelector), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsKey.DependencyProperty;
        public static ICollectionChanged GetSelectedItems(Selector i) => i.GetValueOrSetDefault<ICollectionChanged>(SelectedItemsKey, () => new ObservableCollection<object>());

        #endregion

        #region XSelector

        static XSelector()
        {
            EventManager.RegisterClassHandler(typeof(Selector), Selector.SelectionChangedEvent,
                new SelectionChangedEventHandler(OnSelectionChanged), true);
        }

        #endregion

        #region Methods

        static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Selector selector)
            {
                var selection = GetSelectedItems(selector);
                e.AddedItems?.
                    ForEach(i => selection.Add(i));
                e.RemovedItems?.
                    ForEach(i => selection.Remove(i));
            }
        }

        //...

        public static void ClearSelection(this Selector input) => input.SelectedIndex = -1;

        #endregion
    }
}