using Imagin.Common.Converters;
using Imagin.Common.Data;
using System.IO;

namespace Imagin.Common.Storage
{
    public sealed class Drive : Container
    {
        long availableFreeSpace;
        [DisplayName("Available free space")]
        [Convert(typeof(FileSizeConverter), FileSizeFormat.BinaryUsingSI)]
        [ReadOnly]
        public long AvailableFreeSpace
        {
            get => availableFreeSpace;
            set => this.Change(ref availableFreeSpace, value);
        }

        string format;
        [Hidden]
        public string Format
        {
            get => format;
            set => this.Change(ref format, value);
        }

        [Category("Attributes")]
        [DisplayName("Hidden")]
        [ReadOnly]
        public override bool IsHidden
        {
            get => base.IsHidden;
            set => base.IsHidden = value;
        }

        [Category("Attributes")]
        [DisplayName("ReadOnly")]
        [ReadOnly]
        public override bool IsReadOnly
        {
            get => base.IsReadOnly;
            set => base.IsReadOnly = value;
        }

        long totalSize;
        [DisplayName("Total size")]
        [Convert(typeof(FileSizeConverter), FileSizeFormat.BinaryUsingSI)]
        [ReadOnly]
        public long TotalSize
        {
            get => totalSize;
            set => this.Change(ref totalSize, value);
        }

        public Drive(DriveInfo driveInfo) : base(ItemType.Drive, Origin.Local, driveInfo.Name)
        {
            AvailableFreeSpace
                = driveInfo.AvailableFreeSpace;
            Format
                = driveInfo.DriveFormat;
            TotalSize
                = driveInfo.TotalSize;
        }

        public Drive(string path) : base(ItemType.Drive, Origin.Local, path) { }

        public override FileSystemInfo Read() => null;
    }
}