using Imagin.Common.Mvvm;
using Imagin.Common.Tracing;
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

        ILog GetLog();

        IMainWindowViewModel GetMainWindowViewModel();

        Window MainWindow
        {
            get;
        }
    }
}
