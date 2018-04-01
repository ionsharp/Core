using Imagin.Common.Debug;
using Imagin.Common.Globalization;

namespace Imagin.Common.Configuration
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
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        ILog Log
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        IMainView MainView
        {
            get;
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
