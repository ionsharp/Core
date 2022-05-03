using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class TreeViewRow : TreeViewGrid
    {
        TreeView treeView = null;

        TreeViewItem treeViewItem = null;

        //...

        public TreeViewRow() : base()
        {
            SetCurrentValue(SplitterVisibilityProperty, false);
            this.RegisterHandler(i =>
            {
                treeViewItem
                    = this.FindParent<TreeViewItem>() ?? throw new NotSupportedException();
                treeView
                    = treeViewItem.FindParent<TreeView>();

                if (treeView != null)
                {
                    treeView.RemoveChanged
                        (XTreeView.ModeProperty, OnTreeViewModeChanged);
                    treeView.AddChanged
                        (XTreeView.ModeProperty, OnTreeViewModeChanged);

                    Reset();
                }
            }, i =>
            {
                if (treeView != null)
                {
                    treeView.RemoveChanged(XTreeView.ModeProperty, OnTreeViewModeChanged);
                    Unsubscribe();
                }
            });
        }

        //...

        void Reset()
        {
            Clear(); Load();
        }

        //...

        protected override void Bind(TreeViewColumn column, ColumnDefinition definition)
        {
            base.Bind(column, definition);
            definition.Bind(ColumnDefinition.WidthProperty, nameof(TreeViewColumn.Width), column, BindingMode.OneWay);
        }

        //...

        protected override void Hide(TreeViewColumn column)
        {
            var index 
                = XTreeView.GetColumns(treeView).IndexOf(column);
            var content 
                = Children.OfType<ContentPresenter>().First(i => GetColumn(i) == index);

            Hide(content, index);
        }

        protected override void Show(TreeViewColumn column)
        {
            var index 
                = XTreeView.GetColumns(treeView).IndexOf(column);
            var content 
                = Children.OfType<ContentPresenter>().First(i => GetColumn(i) == index);

            Show(column, content, index);
        }

        //...

        TreeViewCell GetCell(TreeViewColumn column, TreeViewItem item)
        {
            TreeViewCell result = new(column);
            result.Bind(ContentControl.ContentStringFormatProperty,
                nameof(TreeViewColumn.StringFormat), column);
            result.Bind(ContentControl.ContentTemplateProperty,
                nameof(TreeViewColumn.Template), column);
            result.Bind(ContentControl.ContentTemplateSelectorProperty,
                nameof(TreeViewColumn.TemplateSelector), column);

            if (column is TreeViewBindingColumn bindingColumn)
            {
                Binding binding = new(bindingColumn.Member ?? ".")
                {
                    Converter
                        = bindingColumn.Converter,
                    ConverterParameter
                        = bindingColumn.ConverterParameter,
                    Mode
                        = bindingColumn.Mode,
                    Source
                        = item.DataContext
                };
                result.Bind(ContentControl.ContentProperty, binding);
                result.Binding = binding;
            }
            else result.Bind(ContentControl.ContentProperty, ".", item.DataContext);

            result.Bind(HorizontalAlignmentProperty, 
                nameof(column.HorizontalContentAlignment), 
                column);
            result.Bind(VerticalAlignmentProperty, 
                nameof(column.VerticalContentAlignment), 
                column);
            result.Bind(VisibilityProperty, 
                new PropertyPath("(0)", XDependency.IsVisibleProperty), 
                column, BindingMode.OneWay, Converters.BooleanToVisibilityConverter.Default);
            return result;
        }

        void Load()
        {
            if (treeView is not null && treeViewItem is not null)
            {
                treeViewItem.Visibility = Visibility.Collapsed;

                ObjectCollection result = new();
                switch (XTreeView.GetMode(treeView))
                {
                    case TreeViewModes.Default:
                        result.Add(new ContentPresenter()
                        {
                            Content
                                = treeViewItem.Header,
                            ContentStringFormat
                                = treeViewItem.HeaderStringFormat,
                            ContentTemplate
                                = treeViewItem.HeaderTemplate,
                            ContentTemplateSelector
                                = treeViewItem.HeaderTemplateSelector,
                        });
                        break;

                    case TreeViewModes.Grid:
                        var columns = XTreeView.GetColumns(treeView);
                        if (columns?.Count > 0)
                        {
                            columns.For(0, columns.Count, (a, b) => result.Add(GetCell(a[b], treeViewItem)));
                            break;
                        }
                        goto case TreeViewModes.Default;
                }
                ItemsSource = result;

                treeViewItem.Visibility = Visibility.Visible;
            }
        }

        //...

        void OnTreeViewModeChanged(object sender, EventArgs e) => Reset();

        //...

        protected override void Clear()
        {
            Unsubscribe();
            ColumnDefinitions.ForEach(i => Unbind(i));
            base.Clear();
        }

        protected override ColumnDefinition OnDefinitionCreated(ContentPresenter child, int index)
        {
            var result = base.OnDefinitionCreated(child, index);
            switch (XTreeView.GetMode(treeView))
            {
                case TreeViewModes.Default:
                    result.Width = new(1, GridUnitType.Star);
                    break;

                case TreeViewModes.Grid:
                    Bind(XTreeView.GetColumns(treeView)[index], result);
                    break;
            }
            return result;
        }

        protected override void OnItemsSourceChanged(Value<IEnumerable> input)
        {
            base.OnItemsSourceChanged(input);
            if (XTreeView.GetMode(treeView) == TreeViewModes.Grid)
            {
                var columns = XTreeView.GetColumns(treeView);
                if (columns?.Count > 0)
                {
                    columns.ForEach(i =>
                    {
                        if (!XDependency.GetIsVisible(i))
                            Hide(i);
                    });
                }
            }
        }

        //...

        protected override void Subscribe()
        {
            if (treeView != null)
            {
                if (XTreeView.GetMode(treeView) == TreeViewModes.Grid)
                    XTreeView.GetColumns(treeView)?.ForEach(i => i.VisibilityChanged += OnColumnVisibilityChanged);
            }
        }

        protected override void Unsubscribe()
        {
            if (treeView != null)
                XTreeView.GetColumns(treeView)?.ForEach(i => i.VisibilityChanged -= OnColumnVisibilityChanged);
        }
    }
}