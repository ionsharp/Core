using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public abstract class DockLayoutRoot : Base
    {
        List<DockLayoutPanel> top = new();
        [XmlArray]
        [XmlArrayItem(ElementName = "Panel")]
        public List<DockLayoutPanel> Top
        {
            get => top;
            set => this.Change(ref top, value);
        }

        List<DockLayoutPanel> left = new();
        [XmlArray]
        [XmlArrayItem(ElementName = "Panel")]
        public List<DockLayoutPanel> Left
        {
            get => left;
            set => this.Change(ref left, value);
        }

        List<DockLayoutPanel> right = new();
        [XmlArray]
        [XmlArrayItem(ElementName = "Panel")]
        public List<DockLayoutPanel> Right
        {
            get => right;
            set => this.Change(ref right, value);
        }

        List<DockLayoutPanel> bottom = new();
        [XmlArray]
        [XmlArrayItem(ElementName = "Panel")]
        public List<DockLayoutPanel> Bottom
        {
            get => bottom;
            set => this.Change(ref bottom, value);
        }

        DockLayoutElement root;
        public DockLayoutElement Root
        {
            get => root;
            set => this.Change(ref root, value);
        }

        public DockLayoutRoot() : base() { }
    }
}