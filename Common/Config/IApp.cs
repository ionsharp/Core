using Imagin.Common.Debug;
using Imagin.Common.Globalization;

namespace Imagin.Common.Config
{
    /// <summary>
    /// Specifies an application.
    /// </summary>
    public interface IApp
    {
        /// <summary>
        /// 
        /// </summary>
        ILanguages Languages
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        ILog Log
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        IMainView MainView
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        object Resources
        {
            get;
        }
    }
}
