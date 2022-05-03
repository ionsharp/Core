using Imagin.Common.Linq;
using System.IO;
using System.Linq;

namespace Imagin.Common.Storage
{
    public class ItemCollection : StorageCollection<Item>
    {
        public ItemCollection() : base() { }

        public ItemCollection(Filter filter) : base(filter) { }

        public ItemCollection(string path, Filter filter) : base(path, filter) { }

        //...

        protected override Item this[string path] => this.FirstOrDefault(i => i.Path == path);

        protected override ItemProperty GetChangedProperty(Item input)
        {
            //Old
            var oldAccessed
                = input.Accessed;
            var oldCreated
                = input.Created;
            var oldModified
                = input.Modified;
            var oldSize
                = input.Size;

            //New
            input.Refresh();
            ItemProperty result = default;

            if (oldAccessed != input.Accessed)
                result |= ItemProperty.Accessed;

            if (oldCreated != input.Created)
                result |= ItemProperty.Created;

            if (oldModified != input.Modified)
                result |= ItemProperty.Modified;

            if (oldSize != input.Size)
                result |= ItemProperty.Size;

            return result;
        }

        //...

        protected override Item ToDrive(DriveInfo input)
        {
            var result = new Drive(input);
            result.Refresh();
            return result;
        }

        protected override Item ToFile(string input)
        {
            var result = Shortcut.Is(input) ? new Shortcut(input) : new File(input);
            result.Refresh();
            return result;
        }

        protected override Item ToFolder(string input)
        {
            var result = new Folder(input);
            result.Refresh();
            return result;
        }

        //...

        protected override void OnItemCreated(Item i)
        {
            base.OnItemCreated(i);
            i.Refresh();
        }

        protected override void OnItemRenamed(RenamedEventArgs e)
        {
            base.OnItemRenamed(e);
            this[e.OldFullPath]?.Refresh(e.FullPath);
        }

        //...

        protected override void OnAdded(Item input)
        {
            base.OnAdded(input); 
            if (subscribed)
                input.If<Container>(i => i.Items.Subscribe());
        }

        protected override void OnRemoved(Item input)
        {
            base.OnRemoved(input);
            input.If<Container>(i => i.Items.Clear());
        }

        //...

        public override void Subscribe()
        {
            base.Subscribe();
            this.ForEach<Container>
                (i => i.Items.Subscribe());
            this.ForEach<Shortcut>
                (i => i.Items.Subscribe());
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            this.ForEach<Container>
                (i => i.Items.Unsubscribe());
            this.ForEach<Shortcut>
                (i => i.Items.Unsubscribe());
        }
    }
}