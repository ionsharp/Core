using System.Collections.Generic;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface IContainer<TObject>
    {
        /// <summary>
        /// 
        /// </summary>
        IList<TObject> Items
        {
            get; set;
        }
    }
}
