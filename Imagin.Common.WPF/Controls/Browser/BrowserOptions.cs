using Imagin.Common.Data;
using Imagin.Common.Storage;
using System;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    [DisplayName(nameof(Browser))]
    [Serializable]
    public class BrowserOptions : Base
    {
        enum Category
        {
            Filter, Group, Sort, View
        }

        Attributes fileAttributes = Attributes.All;
        [Category(Category.Filter)]
        [DisplayName("FileAttributes")]
        [Style(EnumStyle.FlagSelect)]
        public Attributes FileAttributes
        {
            get => fileAttributes;
            set => this.Change(ref fileAttributes, value);
        }

        string fileExtensions = string.Empty;
        [Category(Category.Filter)]
        [DisplayName("FileExtensions")]
        [Style(StringStyle.Tokens)]
        [UpdateSourceTrigger(UpdateSourceTrigger.LostFocus)]
        public string FileExtensions
        {
            get => fileExtensions;
            set => this.Change(ref fileExtensions, value);
        }

        Attributes folderAttributes = Attributes.All;
        [Category(Category.Filter)]
        [DisplayName("FolderAttributes")]
        [Style(EnumStyle.FlagSelect)]
        public Attributes FolderAttributes
        {
            get => folderAttributes;
            set => this.Change(ref folderAttributes, value);
        }

        SortDirection groupDirection = SortDirection.Ascending;
        [Category(Category.Group)]
        [DisplayName("GroupDirection")]
        public SortDirection GroupDirection
        {
            get => groupDirection;
            set => this.Change(ref groupDirection, value);
        }

        ItemProperty groupName = ItemProperty.None;
        [Category(Category.Group)]
        [DisplayName("GroupName")]
        public ItemProperty GroupName
        {
            get => groupName;
            set => this.Change(ref groupName, value);
        }

        SortDirection sortDirection = SortDirection.Ascending;
        [Category(Category.Sort)]
        [DisplayName("SortDirection")]
        public SortDirection SortDirection
        {
            get => sortDirection;
            set => this.Change(ref sortDirection, value);
        }

        ItemProperty sortName = ItemProperty.Name;
        [Category(Category.Sort)]
        [DisplayName("SortName")]
        public ItemProperty SortName
        {
            get => sortName;
            set => this.Change(ref sortName, value);
        }

        BrowserView view = BrowserView.Thumbnails;
        [Category(Category.View)]
        [DisplayName("View")]
        public BrowserView View
        {
            get => view;
            set => this.Change(ref view, value);
        }

        bool viewCheckBoxes = false;
        [Category(Category.View)]
        [DisplayName("CheckBoxes")]
        public bool ViewCheckBoxes
        {
            get => viewCheckBoxes;
            set => this.Change(ref viewCheckBoxes, value);
        }

        bool viewFileExtensions = false;
        [Category(Category.View)]
        [DisplayName("FileExtensions")]
        public bool ViewFileExtensions
        {
            get => viewFileExtensions;
            set => this.Change(ref viewFileExtensions, value);
        }

        bool viewFiles = true;
        [Category(Category.View)]
        [DisplayName("Files")]
        public bool ViewFiles
        {
            get => viewFiles;
            set => this.Change(ref viewFiles, value);
        }

        double viewSize = 64.0;
        [Category(Category.View)]
        [DisplayName("Size")]
        [Range(8.0, 512.0, 4.0)]
        [Format(RangeFormat.Both)]
        public double ViewSize
        {
            get => viewSize;
            set => this.Change(ref viewSize, value);
        }

        public override string ToString() => nameof(Browser);
    }
}