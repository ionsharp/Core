using System.Collections.Generic;
using System.Windows;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// Represents an application.
    /// </summary>
    public interface IApp
    {
        IEnumerable<string> CommandLineArgs
        {
            get; 
        }

        IAppConfig GetConfig();
        
        Window MainWindow
        {
            get;
        }
    }
}
