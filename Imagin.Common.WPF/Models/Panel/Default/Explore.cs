using Imagin.Common.Controls;
using Imagin.Common.Input;
using System;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    [MemberVisibility(MemberVisibility.Explicit, MemberVisibility.Explicit)]
    [Serializable]
    public class ExplorePanel : Panel
    {
        public static readonly ResourceKey TemplateKey = new();

        [field: NonSerialized]
        public event EventHandler<EventArgs<string>> OpenedFile;

        ExplorerOptions explorerOptions = new();
        [Option, Visible]
        public ExplorerOptions ExplorerOptions
        {
            get => explorerOptions;
            set => this.Change(ref explorerOptions, value);
        }

        public override Uri Icon => Resources.InternalImage(Images.Folder);

        public override string Title => "Explore";

        public ExplorePanel() : base() { }

        void OnOpenedFile(string i) => OpenedFile?.Invoke(this, new EventArgs<string>(i));

        ICommand openedFileCommand;
        public ICommand OpenedFileCommand => openedFileCommand ??= new RelayCommand<string>(i => OnOpenedFile(i), i => i is string);
    }
}