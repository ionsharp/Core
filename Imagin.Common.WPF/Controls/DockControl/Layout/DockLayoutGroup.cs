using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class DockLayoutGroup : DockLayoutElement
    {
        ObservableCollection<DockLayoutElement> elements = new();
        [XmlArray]
        [XmlArrayItem("Element")]
        public ObservableCollection<DockLayoutElement> Elements
        {
            get => elements;
            set => this.Change(ref elements, value);
        }

        Orientation orientation = Orientation.Horizontal;
        [XmlAttribute]
        public Orientation Orientation
        {
            get => orientation;
            set => this.Change(ref orientation, value);
        }

        public DockLayoutGroup() : base() { }

        public DockLayoutGroup(Orientation orientation) : base()
        {
            Orientation = orientation;
        }
    }
}