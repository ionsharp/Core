using System.Collections.Generic;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContainer<T>
    {
        /// <summary>
        /// 
        /// </summary>
        IList<T> Items
        {
            get; set;
        }
    }
}
