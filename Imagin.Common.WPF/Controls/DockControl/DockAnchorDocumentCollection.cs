using Imagin.Common.Collections;
using Imagin.Common.Models;

namespace Imagin.Common.Controls
{
    public class DockAnchorDocumentCollection : DocumentCollection, IDockContentSource
    {
        public DockRootControl Root { get; private set; }

        ICollectionChanged IDockContentSource.Source => this as ICollectionChanged;

        public DockAnchorDocumentCollection(DockRootControl root) => Root = root;
    }
}