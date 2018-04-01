using System.Collections.Generic;

namespace Imagin.Common.IO
{
    /// <summary>
    /// Specifies an object capable of querying system objects.
    /// </summary>
    public interface ISystemObjectProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path">The path to query.</param>
        /// <param name="Source">A source used to make queries.</param>
        /// <returns>A list of system object paths.</returns>
        IEnumerable<string> Query(string Path, object Source = null);
    }
}
