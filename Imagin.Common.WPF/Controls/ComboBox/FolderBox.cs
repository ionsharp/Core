using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class FolderBox : ComboBox, IExplorer
    {
        readonly FolderBoxDropHandler DropHandler;

        readonly Storage.ItemCollection items = new(Filter.Default);

        static object OnItemsSourceCoerced(DependencyObject sender, object input)
        {
            if (sender is FolderBox box)
            {
                if (input != box.items)
                    throw new NotSupportedException();
            }
            return input;
        }

        public string Path
        {
            get => XExplorer.GetPath(this);
            set => XExplorer.SetPath(this, value);
        }

        static FolderBox()
        {
            ItemsSourceProperty.OverrideMetadata(typeof(FolderBox), new FrameworkPropertyMetadata(null, null, OnItemsSourceCoerced));
        }

        public FolderBox() : base()
        {
            DropHandler = new(this);
            GongSolutions.Wpf.DragDrop.DragDrop.SetDropHandler(this, DropHandler);

            this.RegisterHandler(i =>
            {
                items.Subscribe();
                _ = items.RefreshAsync(Path);

                i.AddPathChanged(OnPathChanged);
            },
            i =>
            {
                items.Unsubscribe();
                items.Clear();

                i.RemovePathChanged(OnPathChanged);
            });

            SetCurrentValue(ItemsSourceProperty, items);
        }

        protected virtual void OnPathChanged(object sender, PathChangedEventArgs e) => _ = items.RefreshAsync(e.Path);
    }
}