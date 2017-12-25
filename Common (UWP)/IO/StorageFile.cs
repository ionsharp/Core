using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Foundation;
using Windows.Storage.FileProperties;

namespace Imagin.Common.IO
{
    public class SystemFile : SystemItem<StorageFile>
    {
        public SystemFile(StorageFile Source) : base(Source)
        {
        }
    }

    public class SystemFolder : SystemItem<StorageFolder>
    {
        public SystemFolder(StorageFolder Source) : base(Source)
        {
        }
    }

    public class SystemItem<TStorageItem> : SystemItem where TStorageItem : IStorageItem
    {
        public SystemItem(TStorageItem Source) : base()
        {
            Attributes = Source.Attributes;
            DateCreated = Source.DateCreated;
            Name = Source.Name;
            Path = Source.Path;
        }
    }

    public class SystemItem : IStorageItem
    {
        public FileAttributes Attributes
        {
            get; protected set;
        }

        public DateTimeOffset DateCreated
        {
            get; protected set;
        }

        public string Name
        {
            get; protected set;
        }

        public string Path
        {
            get; protected set;
        }

        public SystemItem()
        {
        }

        IAsyncAction IStorageItem.DeleteAsync()
        {
            return default(IAsyncAction);
        }

        IAsyncAction IStorageItem.DeleteAsync(StorageDeleteOption option)
        {
            return default(IAsyncAction);
        }

        IAsyncOperation<BasicProperties> IStorageItem.GetBasicPropertiesAsync()
        {
            return default(IAsyncOperation<BasicProperties>);
        }

        bool IStorageItem.IsOfType(StorageItemTypes ItemType)
        {
            return default(bool);
        }

        IAsyncAction IStorageItem.RenameAsync(string desiredName)
        {
            return default(IAsyncAction);
        }

        IAsyncAction IStorageItem.RenameAsync(string desiredName, NameCollisionOption option)
        {
            return default(IAsyncAction);
        }
    }
}
