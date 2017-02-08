using Imagin.Common;
using System.Windows.Controls;

namespace Imagin.Controls.Common
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
