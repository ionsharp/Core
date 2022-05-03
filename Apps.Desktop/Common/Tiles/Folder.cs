using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Storage;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;

namespace Imagin.Apps.Desktop
{
    [DisplayName("Folder")]
    [Serializable]
    public class FolderTile : Tile, IFrameworkReference
    {
        [field: NonSerialized]
        public static readonly ReferenceKey<Browser> BrowserReferenceKey = new();

        [Hidden, XmlIgnore]
        public Browser Browser { get; private set; }

        BrowserOptions browserOptions = new();
        [Style(ObjectStyle.Expander)]
        public BrowserOptions BrowserOptions
        {
            get => browserOptions;
            set => this.Change(ref browserOptions, value);
        }

        bool isReadOnly = true;
        public bool IsReadOnly
        {
            get => isReadOnly;
            set => this.Change(ref isReadOnly, value);
        }

        string path = StoragePath.Root;
        [Hidden]
        public string Path
        {
            get => path;
            set => this.Change(ref path, value);
        }

        [Hidden, XmlIgnore]
        public override string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        public FolderTile() : base() { }

        public FolderTile(string path) : base()
        {
            Path = path;
        }

        void IFrameworkReference.SetReference(IFrameworkKey key, FrameworkElement element)
        {
            if (key == BrowserReferenceKey)
                Browser = (Browser)element;
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Path):
                    OnChanged();
                    break;
            }
        }
    }
}