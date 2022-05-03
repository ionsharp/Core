using Imagin.Common.Data;
using Imagin.Common.Storage;
using System;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    [DisplayName(nameof(Navigator))]
    [Serializable]
    public class NavigatorOptions : Base
    {
        enum Category
        {
            Filter,
            Mode,
            Sort,
            View,
            Visibility
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

        TreeViewModes mode = TreeViewModes.Default;
        [Category(Category.Mode)]
        public TreeViewModes Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
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

        Visibility visibility = Visibility.Collapsed;
        [Category(Category.Visibility)]
        [DisplayName("Visibility")]
        public Visibility Visibility
        {
            get => visibility;
            set => this.Change(ref visibility, value);
        }

        public override string ToString() => nameof(Navigator);
    }
}