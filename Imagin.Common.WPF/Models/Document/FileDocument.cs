using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using Imagin.Common.Converters;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Imagin.Common.Models
{
    [DisplayName("File Document")]
    [Description("Interface for managing a file.")]
    [Serializable]
    public abstract class FileDocument : Document
    {
        enum Category { File, FileAttributes, FileProperties }

        #region Properties

        [field: NonSerialized]
        DateTime accessed = DateTime.Now;
        [Category(Category.FileProperties)]
        [Convert(typeof(RelativeTimeConverter))]
        [Description("When the file was last accessed.")]
        [ReadOnly]
        [XmlIgnore]
        public DateTime Accessed
        {
            get => accessed;
            private set => this.Change(ref accessed, value);
        }

        [field: NonSerialized]
        DateTime created = DateTime.Now;
        [Category(Category.FileProperties)]
        [Convert(typeof(RelativeTimeConverter))]
        [Description("When the file was created.")]
        [ReadOnly]
        [XmlIgnore]
        public DateTime Created
        {
            get => created;
            private set => this.Change(ref created, value);
        }

        [Hidden, XmlIgnore]
        public virtual string Extension => System.IO.Path.GetExtension(path);

        [Hidden, XmlIgnore]
        public virtual string[] Extensions => Array<string>.New(Extension);

        [Category(Category.File)]
        [DisplayName("Extension")]
        [ReadOnly, XmlIgnore]
        public string ExtensionDescription => File.Long.Description($".{Extension}");

        [Hidden]
        public override object Icon => path;

        [field: NonSerialized]
        bool isHidden = false;
        [Category(Category.FileAttributes)]
        [Description("If the file is hidden or not.")]
        [DisplayName("Hidden")]
        [XmlIgnore, ReadOnly]
        public virtual bool IsHidden
        {
            get => isHidden;
            set
            {
                Try.Invoke(() =>
                {
                    if (value)
                        File.Long.AddAttribute(path, System.IO.FileAttributes.Hidden);

                    else File.Long.RemoveAttribute(path, System.IO.FileAttributes.Hidden);
                    isHidden = value;
                },
                e => Log.Write<FileDocument>(e));
                this.Changed(() => IsHidden);
            }
        }

        [field: NonSerialized]
        bool isReadOnly = false;
        [Category(Category.FileAttributes)]
        [Description("If the file is read only or not.")]
        [DisplayName("ReadOnly")]
        [XmlIgnore, ReadOnly]
        public virtual bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                Try.Invoke(() =>
                {
                    if (value)
                        File.Long.AddAttribute(path, System.IO.FileAttributes.ReadOnly);

                    else File.Long.RemoveAttribute(path, System.IO.FileAttributes.ReadOnly);
                    isReadOnly = value;
                },
                e => Log.Write<FileDocument>(e));
                this.Changed(() => IsReadOnly);
            }
        }

        [field: NonSerialized]
        DateTime modified = DateTime.Now;
        [Category(Category.FileProperties)]
        [Convert(typeof(RelativeTimeConverter))]
        [Description("When the file was last modified.")]
        [ReadOnly]
        [Status]
        [XmlIgnore]
        public DateTime Modified
        {
            get => modified;
            private set => this.Change(ref modified, value);
        }

        [Featured]
        [UpdateSourceTrigger(UpdateSourceTrigger.LostFocus)]
        [XmlIgnore]
        public string Name
        {
            get => System.IO.Path.GetFileNameWithoutExtension(path);
            set
            {
                value = StoragePath.CleanName(value).TrimWhitespace();
                if (value.NullOrEmpty())
                    value = $"Untitled";

                value += $".{Extension}";

                var folderPath = System.IO.Path.GetDirectoryName(path);
                if (Folder.Long.Exists(folderPath))
                {
                    var oldPath = path;
                    var newPath = $@"{folderPath}\{value}.{Extension}";

                    if (!File.Long.Exists(newPath))
                    {
                        if (Try.Invoke(() => System.IO.File.Move(oldPath, newPath), e => Log.Write<FileDocument>(e)))
                            value = newPath;

                        else goto skip;
                    }
                    else
                    {
                        Log.Write<FileDocument>(new Error("A file with that name already exists!"));
                        goto skip;
                    }

                    goto handle;
                    skip: 
                    { 
                        this.Changed(() => Name); 
                        return; 
                    }
                }
                handle: Path = value;
            }
        }

        string path;
        [Hidden]
        [Index(-1)]
        [Label(false)]
        [Style(StringStyle.Thumbnail)]
        [Status]
        [XmlIgnore]
        public string Path
        {
            get => path;
            set => this.Change(ref path, value);
        }

        long size = 0;
        [Category(Category.File)]
        [Convert(typeof(FileSizeConverter), FileSizeFormat.BinaryUsingSI)]
        [Description("The size of the file (in bytes).")]
        [ReadOnly]
        [XmlIgnore]
        public long Size
        {
            get => size;
            private set => this.Change(ref size, value);
        }

        [Hidden, XmlIgnore]
        public override string Title => $"{System.IO.Path.GetFileNameWithoutExtension(path)}{(IsModified ? "*" : string.Empty)}";

        [Hidden, XmlIgnore]
        public override object ToolTip => Path;

        #endregion

        #region FileDocument

        public FileDocument() : base() { }

        #endregion

        #region Methods

        bool CheckBusy()
        {
            if (IsBusy)
            {
                Dialog.Show("Save", "The document is already saving.", DialogImage.Exclamation, Buttons.Ok);
                return true;
            }
            return false;
        }

        async void Save(string filePath)
        {
            if (CheckBusy()) return;
            IsModified = false;

            IsBusy = true;

            var result = await SaveAsync(filePath);

            var newPath = result ? filePath : null;
            if (newPath != null && Extension == System.IO.Path.GetExtension(newPath).Substring(1))
                Path = newPath;

            IsBusy = false;
        }

        void SaveAs()
        {
            var result = StorageWindow.Show(out string path, "SaveAs".Translate() + "...", StorageWindowModes.SaveFile, Extensions, Path, StorageWindowFilterModes.Alphabetical);
            if (result) Save(path);
        }

        protected abstract Task<bool> SaveAsync(string filePath);

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(IsModified):
                    if (!IsModified)
                        Modified = DateTime.Now;

                    break;

                case nameof(Path):

                    Try.Invoke(() =>
                    {
                        var fileInfo = new System.IO.FileInfo(path);
                        Accessed
                            = fileInfo.LastAccessTime;
                        Created
                            = fileInfo.CreationTime;
                        Modified
                            = fileInfo.LastWriteTime;
                        Size
                            = fileInfo.Length;
                    });

                    this.Changed(() => Name);
                    this.Changed(() => Title);
                    this.Changed(() => ToolTip);
                    break;
            }
        }

        public sealed override void Save()
        {
            if (CheckBusy()) return;
            if (!File.Long.Exists(Path))
                SaveAs();

            else Save(Path);
        }

        #endregion

        #region Commands

        [DisplayName("Save")]
        [Icon(Images.Save)]
        [Tool]
        [XmlIgnore]
        public override ICommand SaveCommand => base.SaveCommand;

        [field: NonSerialized]
        ICommand saveAsCommand;
        [DisplayName("SaveAs")]
        [Icon(Images.SaveAs)]
        [Tool]
        [XmlIgnore]
        public ICommand SaveAsCommand => saveAsCommand ??= new RelayCommand(SaveAs);

        #endregion
    }
}