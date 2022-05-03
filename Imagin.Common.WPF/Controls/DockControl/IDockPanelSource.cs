using Imagin.Common.Collections;

namespace Imagin.Common.Controls
{
    public interface IDockPanelSource
    {
        DockRootControl Root { get; }

        ICollectionChanged Source { get; }
    }
}