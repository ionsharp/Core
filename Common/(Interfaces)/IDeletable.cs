using System;

namespace Imagin.Common
{
    public interface IDeletable
    {
        event EventHandler<EventArgs> Deleted;

        void OnDeleted();
    }
}
