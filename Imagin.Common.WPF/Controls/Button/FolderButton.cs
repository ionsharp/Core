using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class FolderButton : ImageButton, IExplorer
    {
        readonly ContextMenu DefaultMenu;

        readonly FolderButtonDropHandler DropHandler;

        public Storage.ItemCollection Items { get; private set; } = new(Filter.Default);

        public static readonly DependencyProperty ItemStyleProperty = DependencyProperty.Register(nameof(ItemStyle), typeof(Style), typeof(FolderButton), new FrameworkPropertyMetadata(null));
        public Style ItemStyle
        {
            get => (Style)GetValue(ItemStyleProperty);
            set => SetValue(ItemStyleProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(FolderButton), new FrameworkPropertyMetadata(null));
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register(nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(FolderButton), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector ItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            set => SetValue(ItemTemplateSelectorProperty, value);
        }

        public string Path
        {
            get => XExplorer.GetPath(this);
            set => XExplorer.SetPath(this, value);
        }

        public FolderButton() : base()
        {
            DropHandler = new(this);
            GongSolutions.Wpf.DragDrop.DragDrop.SetDropHandler(this, DropHandler);

            this.RegisterHandler(i =>
            {
                Items.Subscribe();
                _ = Items.RefreshAsync(Path);

                i.AddPathChanged(OnPathChanged);
            },
            i =>
            {
                Items.Unsubscribe();
                Items.Clear();

                i.RemovePathChanged(OnPathChanged);
            });

            DefaultMenu = new();
            DefaultMenu.Bind(ContextMenu.ItemContainerStyleProperty, 
                nameof(ItemStyle), 
                this);
            DefaultMenu.Bind(ContextMenu.ItemsSourceProperty, 
                nameof(Items), 
                this);
            DefaultMenu.Bind(ContextMenu.ItemTemplateProperty,
                nameof(ItemTemplate),
                this);
            DefaultMenu.Bind(ContextMenu.ItemTemplateSelectorProperty,
                nameof(ItemTemplateSelector),
                this);

            SetCurrentValue(MenuProperty, DefaultMenu);
        }

        protected override void OnMenuChanged(Value<ContextMenu> input)
        {
            base.OnMenuChanged(input);
            if (input.Old is not null)
                throw new NotSupportedException();
        }

        protected virtual void OnPathChanged(object sender, PathChangedEventArgs e) => _ = Items.RefreshAsync(e.Path);
    }
}