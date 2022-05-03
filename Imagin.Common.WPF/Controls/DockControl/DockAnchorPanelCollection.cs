using Imagin.Common.Collections;
using Imagin.Common.Models;

namespace Imagin.Common.Controls
{
    public class DockAnchorPanelCollection : PanelCollection, IDockContentSource, IDockPanelSource
    {
        public DockRootControl Root { get; private set; }

        ICollectionChanged IDockContentSource.Source => this;

        ICollectionChanged IDockPanelSource.Source => this;

        public DockAnchorPanelCollection(DockRootControl root) => Root = root;
    }
}