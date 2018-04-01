using Imagin.Common;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NamedRule : ValidationRule, INamable
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get; set;
        }
    }
}
