using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public abstract class TreeViewGrid : HorizontalGrid
    {
        public TreeViewGrid() : base() { }

        //...

        protected void OnColumnVisibilityChanged(object sender, EventArgs e)
        {
            if (sender is TreeViewColumn column)
            {
                if (column.Visibility == Visibility.Visible)
                    Show(column);

                else Hide(column);
            }
        }

        protected override void OnItemsSourceChanged(Value<IEnumerable> input)
        {
            Unsubscribe();
            base.OnItemsSourceChanged(input);
            Subscribe();
        }

        //...

        protected virtual void Bind(TreeViewColumn column, ColumnDefinition definition)
        {
            definition.Bind(ColumnDefinition.MaxWidthProperty,
                nameof(TreeViewColumn.MaxWidth), column);
            definition.Bind(ColumnDefinition.MinWidthProperty,
                nameof(TreeViewColumn.MinWidth), column);
        }

        protected void Unbind(ColumnDefinition input)
        {
            input.Unbind(ColumnDefinition.MaxWidthProperty);
            input.Unbind(ColumnDefinition.MinWidthProperty);
            input.Unbind(ColumnDefinition.WidthProperty);
            input.Width = new GridLength(0, GridUnitType.Pixel);
        }

        //...

        protected void Hide(ContentPresenter content, int index)
        {
            content.Visibility = Visibility.Collapsed;

            var actualIndex = Children.IndexOf(content);
            if (actualIndex + 1 < Children.Count)
                Children[actualIndex + 1].Visibility = Visibility.Collapsed;

            Unbind(ColumnDefinitions[index]);
        }

        protected void Show(TreeViewColumn column, ContentPresenter content, int index)
        {
            content.Visibility = Visibility.Visible;

            var actualIndex = Children.IndexOf(content);
            if (actualIndex + 1 < Children.Count)
                Children[actualIndex + 1].Visibility = Visibility.Visible;

            Bind(column, ColumnDefinitions[index]);
        }

        //...

        protected abstract void Hide(TreeViewColumn column);

        protected abstract void Show(TreeViewColumn column);

        //...

        protected abstract void Subscribe();

        protected abstract void Unsubscribe();
    }
}