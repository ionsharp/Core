using Imagin.Common;
using System;

namespace Imagin.Apps.Paint
{
    [DisplayName("Group layer")]
    [Icon(App.ImagePath + "LayerGroup.png")]
    [Serializable]
    public class GroupLayer : StyleLayer
    {
        bool isExpanded = true;
        [Hidden]
        public bool IsExpanded
        {
            get => isExpanded;
            set => this.Change(ref isExpanded, value);
        }

        LayerCollection layers = new();
        [Hidden]
        public LayerCollection Layers
        {
            get => layers;
            set => this.Change(ref layers, value);
        }

        public GroupLayer() : this("Untitled") { }

        public GroupLayer(string name, GroupLayer parent = null) : base(LayerType.Group, name) => Parent = parent;

        public override Layer Clone() => new GroupLayer(Name);
    }
}