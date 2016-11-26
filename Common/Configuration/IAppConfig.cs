using Imagin.Common.Mvvm;
using Imagin.Common.Tracing;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// Represents an application configuration object (ACO).
    /// </summary>
    public interface IAppConfig
    {
        ILog GetLog();

        IMainWindowViewModel GetMainWindowViewModel();
    }
}
