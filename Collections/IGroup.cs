using System.Collections;

namespace Imagin.Core.Collections
{
    public interface IGroup : IList 
    {
        string Name { get; set; }
    }
}