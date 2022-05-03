using Imagin.Common.Collections;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class DockLayoutDocumentGroup : DockLayoutElement
    {
        public bool Default { get; set; }

        [XmlIgnore]
        public readonly List<Document> Documents = new();

        List<DockLayoutPanel> panels = new();
        [XmlArray]
        public List<DockLayoutPanel> Panels
        {
            get => panels;
            set => this.Change(ref panels, value);
        }

        public DockLayoutDocumentGroup() : base() { }

        public DockLayoutDocumentGroup(params Content[] content) : this((IEnumerable<Content>)content) { }

        public DockLayoutDocumentGroup(IEnumerable<Content> content) : base()
        {
            foreach (var i in content)
            {
                if (i is Document j)
                    Documents.Add(j);

                else if (i is Panel k)
                    panels.Add(new DockLayoutPanel(k.Name));
            }
        }

        public DockLayoutDocumentGroup(params Document[] documents) 
            : this((IEnumerable<Document>)documents) { }

        public DockLayoutDocumentGroup(IEnumerable<Document> documents) : base() 
            => documents.ForEach(i => Documents.Add(i));

        public DockLayoutDocumentGroup(ICollectionChanged input) 
            : this((IEnumerable<Content>)input) { }
    }
}