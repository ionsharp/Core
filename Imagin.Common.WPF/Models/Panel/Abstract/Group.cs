using Imagin.Common.Collections.Generic;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    public interface IGroupPanel { object SelectedItem { get; } }

    public abstract class GroupPanel<T> : Panel, IGroupPanel where T : new()
    {
        enum Category { Export, Import }

        [Hidden]
        public GroupCollection<T> SelectedGroup => SelectedGroupIndex >= 0 && SelectedGroupIndex < Groups?.Count ? Groups[SelectedGroupIndex] : default;

        object IGroupPanel.SelectedItem => SelectedItem;
        [Hidden]
        public T SelectedItem => SelectedIndex >= 0 && SelectedIndex < SelectedGroup?.Count ? SelectedGroup[SelectedIndex] : default;

        int selectedGroupIndex = -1;
        [Featured, Index(2)]
        [Label(false)]
        [Source(nameof(Groups), nameof(GroupCollection<T>.Name))]
        [Style(Int32Style.Index)]
        [Tool, Visible]
        public int SelectedGroupIndex
        {
            get => selectedGroupIndex;
            set
            {
                this.Change(ref selectedGroupIndex, value);
                this.Changed(() => SelectedGroup);
            }
        }

        int selectedIndex = -1;
        [Hidden]
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                this.Change(ref selectedIndex, value);
                this.Changed(() => SelectedItem);
            }
        }

        GroupWriter<T> groups = null;
        [Hidden]
        public GroupWriter<T> Groups
        {
            get => groups;
            set => this.Change(ref groups, value);
        }

        //...

        public GroupPanel() : base() { }

        //...

        ICommand addCommand;
        [DisplayName("Add")]
        [Icon(Images.Plus)]
        [Index(0)]
        [Tool, Visible]
        public ICommand AddCommand => addCommand ??= new RelayCommand(() => SelectedGroup.As<GroupCollection<T>>().Add(new T()), () => SelectedGroup != null);

        ICommand cloneCommand;
        [DisplayName("Clone")]
        [Icon(Images.Clone)]
        [Index(1)]
        [Tool, Visible]
        public ICommand CloneCommand => cloneCommand ??= new RelayCommand(() => SelectedGroup.Insert(SelectedGroup.IndexOf(SelectedItem), (T)SelectedItem.As<ICloneable>().Clone()), () => SelectedGroup != null && SelectedItem is ICloneable);

        ICommand deleteCommand;
        [DisplayName("Delete")]
        [Icon(Images.Trash)]
        [Index(2)]
        [Tool, Visible]
        public ICommand DeleteCommand => deleteCommand ??= new RelayCommand(() => SelectedGroup.Remove(SelectedItem), () => SelectedGroup?.Contains(SelectedItem) == true);

        ICommand addGroupCommand;
        [DisplayName("Add group")]
        [Featured, Index(0)]
        [Icon(Images.FolderAdd)]
        [Tool, Visible]
        public ICommand AddGroupCommand => addGroupCommand ??= new RelayCommand(() => Groups.Add(new GroupCollection<T>()), () => Groups != null);

        ICommand deleteGroupCommand;
        [DisplayName("Delete group")]
        [Featured, Index(1)]
        [Icon(Images.FolderDelete)]
        [Tool, Visible]
        public ICommand DeleteGroupCommand => deleteGroupCommand ??= new RelayCommand(() => Groups.Remove(SelectedGroup), () => Groups?.Contains(SelectedGroup) == true);

        ICommand exportCommand;
        [Category(nameof(Category.Export))]
        [DisplayName("Export")]
        [Icon(Images.Export)]
        [Tool, Visible]
        public ICommand ExportCommand
            => exportCommand ??= new RelayCommand(() => _ = Groups.Export(SelectedGroup), () => SelectedGroup != null);

        ICommand exportAllCommand;
        [Category(nameof(Category.Export))]
        [DisplayName("ExportAll")]
        [Icon(Images.ExportAll)]
        [Tool, Visible]
        public ICommand ExportAllCommand
            => exportAllCommand ??= new RelayCommand(() => _ = Groups.Export());

        ICommand importCommand;
        [Category(nameof(Category.Import))]
        [DisplayName("Import")]
        [Icon(Images.Import)]
        [Tool, Visible]
        public ICommand ImportCommand
            => importCommand ??= new RelayCommand(() => Groups.Import());
    }
}