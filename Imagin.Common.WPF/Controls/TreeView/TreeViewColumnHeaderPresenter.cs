using Imagin.Common.Linq;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class TreeViewColumnHeaderPresenter : TreeViewGrid
    {
        public TreeViewColumnHeaderPresenter() : base()
            => this.RegisterHandler(i => Subscribe(), i => Unsubscribe());

        //...

        protected override void Bind(TreeViewColumn column, ColumnDefinition definition)
        {
            base.Bind(column, definition);
            definition.Bind(ColumnDefinition.WidthProperty, nameof(TreeViewColumn.Width), column, BindingMode.TwoWay);
        }

        //...

        protected override void Hide(TreeViewColumn column)
        {
            var content 
                = Children.OfType<ContentPresenter>().First(i => i.Content == column);
            var index 
                = GetColumn(content);

            Hide(content, index);
        }

        protected override void Show(TreeViewColumn column)
        {
            var content 
                = Children.OfType<ContentPresenter>().First(i => i.Content == column);
            var index 
                = GetColumn(content);

            Show(column, content, index);
        }

        //...

        protected override ColumnDefinition OnDefinitionCreated(ContentPresenter child, int index)
        {
            var result = base.OnDefinitionCreated(child, index);
            if (child.Content is TreeViewColumn column)
                Bind(column, result);

            return result;
        }

        //...

        protected override void Subscribe() => Children.OfType<ContentPresenter>().Select(i => i.Content as TreeViewColumn).ForEach(i => i.VisibilityChanged += OnColumnVisibilityChanged);

        protected override void Unsubscribe() => Children.OfType<ContentPresenter>().Select(i => i.Content as TreeViewColumn).ForEach(i => i.VisibilityChanged -= OnColumnVisibilityChanged);
    }
}