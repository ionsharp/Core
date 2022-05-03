using Imagin.Common.Linq;
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Controls
{
    /// <author>Unknown</author>
    /// <url>https://stackoverflow.com/questions/9794151/stop-tabcontrol-from-recreating-its-children</url>
    /// <notes>Renamed to <see cref="CacheTabControl"/>.</notes>
    [TemplatePart(Name = nameof(Grid0), Type = typeof(Grid))]
    public class CacheTabControl : TabControl
    {
        public static readonly ReferenceKey<Panel> ContentKey = new();

        Panel Grid0 = null;

        public CacheTabControl() : base()
        {
            ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;
        }

        //...

        void OnItemContainerGeneratorStatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                ItemContainerGenerator.StatusChanged -= OnItemContainerGeneratorStatusChanged;
                Update();
            }
        }

        void Update()
        {
            if (Grid0 == null)
                return;

            //Generate a ContentPresenter if necessary
            var item = GetSelectedTabItem();
            if (item != null)
                CreateChild(item);

            //Show the right child
            foreach (ContentPresenter child in Grid0.Children)
                child.Visibility = ((child.Tag as TabItem).IsSelected) ? Visibility.Visible : Visibility.Collapsed;
        }

        ContentPresenter CreateChild(object item)
        {
            if (item == null)
                return null;

            var content = FindChild(item);

            if (content != null)
                return content;

            // the actual child to be added.  cp.Tag is a reference to the TabItem
            content = new ContentPresenter { Content = (item is TabItem) ? (item as TabItem).Content : item };
            content.Bind(ContentPresenter.ContentTemplateProperty,
                nameof(SelectedContentTemplate), this, System.Windows.Data.BindingMode.OneWay);
            content.Bind(ContentPresenter.ContentTemplateSelectorProperty,
                nameof(SelectedContentTemplateSelector), this, System.Windows.Data.BindingMode.OneWay);
            content.Bind(ContentPresenter.ContentStringFormatProperty,
                nameof(SelectedContentStringFormat), this, System.Windows.Data.BindingMode.OneWay);

            content.Visibility = Visibility.Collapsed;
            content.Tag = (item is TabItem) ? item : (ItemContainerGenerator.ContainerFromItem(item));

            Grid0.Children.Add(content);
            return content;
        }

        ContentPresenter FindChild(object data)
        {
            if (data is TabItem)
                data = (data as TabItem).Content;

            if (data == null)
                return null;

            if (Grid0 == null)
                return null;

            foreach (ContentPresenter i in Grid0.Children)
            {
                if (i.Content == data)
                    return i;
            }

            return null;
        }

        //...

        /// <summary>
        /// When the items change we remove any generated panel children and add any new ones as necessary
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (Grid0 == null)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    Grid0.Children.Clear();
                    break;

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            ContentPresenter cp = FindChild(item);
                            if (cp != null)
                                Grid0.Children.Remove(cp);
                        }
                    }

                    //Don't do anything with new items because we don't want to create visuals that aren't being shown
                    break;

                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();
            }
            Update();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            Update();
        }

        protected TabItem GetSelectedTabItem()
        {
            object selectedItem = SelectedItem;
            if (selectedItem == null)
                return null;

            var item = selectedItem as TabItem;
            if (item == null)
                item = ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;

            return item;
        }

        //...

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Grid0 = this.GetChild(ContentKey) ?? Template.FindName(nameof(Grid0), this) as Grid;
            Grid0?.Children.Clear();
            Update();
        }
    }
}