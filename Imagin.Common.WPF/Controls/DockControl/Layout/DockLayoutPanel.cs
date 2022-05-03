using System;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class DockLayoutPanel : Base
    {
        bool isSelected;
        [XmlAttribute]
        public bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }

        string name;
        [XmlAttribute]
        public string Name
        {
            get => name;
            set => this.Change(ref name, value);
        }

        DockLayoutPanel() : base() { }

        public DockLayoutPanel(string name) : base() => Name = name;

        public DockLayoutPanel(Models.Panel panel) : this(panel.Name) => IsSelected = panel.IsSelected;
    }
}