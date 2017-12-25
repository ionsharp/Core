using System.Collections.Generic;

namespace Imagin.Common.IO
{
    /// <summary>
    /// Defines base functionality for an <see cref="ISystemObjectProvider"/>.
    /// </summary>
    public abstract class SystemObjectProvider : ISystemObjectProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public abstract IEnumerable<string> Query(string Path, object Source = null);
    }
}