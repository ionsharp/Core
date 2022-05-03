using System;
using System.Collections.ObjectModel;

namespace Imagin.Common.Models
{
    public class PropertiesPanel : Panel
    {
        public static readonly ResourceKey TemplateKey = new();

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Properties);

        int selectedIndex = -1;
        [Hidden]
        [Serialize(false)]
        public virtual int SelectedIndex
        {
            get => selectedIndex;
            set => this.Change(ref selectedIndex, value);
        }

        object source = null;
        [Hidden]
        [Serialize(false)]
        public virtual object Source
        {
            get => source;
            set => this.Change(ref source, value);
        }

        ObservableCollection<object> sources = new();
        [Hidden]
        [Serialize(false)]
        public virtual ObservableCollection<object> Sources
        {
            get => sources;
            set => this.Change(ref sources, value);
        }

        [Hidden]
        public override string Title => "Properties";

        public PropertiesPanel() : base() { }
    }
}