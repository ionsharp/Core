using System;
using System.Collections.ObjectModel;

namespace Imagin.Common.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumCollection : ObservableCollection<Enum>
    {
        /// <summary>
        /// 
        /// </summary>
        public EnumCollection() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Values"></param>
        public EnumCollection(params Enum[] Values) : base(Values)
        {
        }
    }
}
