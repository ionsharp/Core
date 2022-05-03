using Imagin.Common.Collections;

namespace Imagin.Common.Controls
{
    public interface IDockContentSource
    {
        DockRootControl Root { get; }

        ICollectionChanged Source { get; }
    }
}