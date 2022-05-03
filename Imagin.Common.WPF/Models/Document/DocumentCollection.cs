using Imagin.Common.Collections.Generic;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Models
{
    [Serializable]
    public class DocumentCollection : ObservableCollection<Document>
    {
        public DocumentCollection() : base() { }

        public DocumentCollection(params Document[] input) : base(input) { }

        public DocumentCollection(IEnumerable<Document> input) : base(input) { }
    }
}