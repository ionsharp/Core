using Imagin.Common.Collections.Concurrent;

namespace Imagin.Common
{
    public interface IContainer
    {
        AbstractObjectCollection Items
        {
            get; set;
        }
    }
}
