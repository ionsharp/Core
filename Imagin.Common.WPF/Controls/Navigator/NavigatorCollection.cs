using Imagin.Common.Collections.Generic;
using Imagin.Common.Storage;

namespace Imagin.Common.Controls
{
    public class NavigatorCollection : ObservableCollection<NavigatorGroup>
    {
        public static NavigatorCollection Default => new()
        {
            new FavoriteGroup(null),
            new FolderGroup(StoragePath.Root)
        };
    }
}