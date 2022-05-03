using System.Collections;

namespace Imagin.Common.Collections
{
    public interface IGroup : IList 
    {
        string Name { get; set; }
    }
}