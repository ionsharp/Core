using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class DockLayoutPanelGroup : DockLayoutElement
    {
        bool collapse = false;
        [XmlAttribute]
        public bool Collapse
        {
            get => collapse;
            set => this.Change(ref collapse, value);
        }

        List<DockLayoutPanel> panels = new();
        [XmlArray]
        public List<DockLayoutPanel> Panels
        {
            get => panels;
            set => this.Change(ref panels, value);
        }

        public DockLayoutPanelGroup() : base() { }

        public DockLayoutPanelGroup(IEnumerable<DockLayoutPanel> input) : base() => input?.ForEach(i => panels.Add(i));

        public DockLayoutPanelGroup(params DockLayoutPanel[] input) : this(input as IEnumerable<DockLayoutPanel>) { }

        public DockLayoutPanelGroup(params Panel[] input) : this(input?.Select(i => new DockLayoutPanel((i as Panel).Name))) { }

        public DockLayoutPanelGroup(IEnumerable<Panel> input) : this(input?.ToArray()) { }

        public DockLayoutPanelGroup(params string[] input) : this(input?.Select(i => new DockLayoutPanel(i.ToString()))) { }
    }
}