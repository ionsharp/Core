using Imagin.Common.Tracing;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Imagin.Common.Config
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Source"></param>
    public delegate void AppInitializerDelegate(IAppStarter Source);

    /// <summary>
    /// Specifies an application.
    /// </summary>
    public interface IApp
    {
        /// <summary>
        /// Occurs just after <see cref="Application.Run()"/> is called.
        /// </summary>
        event EventHandler<EventArgs> Ran;

        /// <summary>
        /// Occurs when the application has finished all startup tasks and is ready to display the main module.
        /// </summary>
        event EventHandler<EventArgs> Started;

        /// <summary>
        /// Gets or sets command line arguments passed to the current application instance.
        /// </summary>
        IEnumerable<string> Arguments
        {
            get; 
        }

        /// <summary>
        /// Gets or sets delegate for assisting with initialization.
        /// </summary>
        AppInitializerDelegate Initializer
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
        /// Gets or sets the main module (<see cref="Window"/>, if WPF).
        /// </summary>
        IMainModule MainModule
        {
            get;
        }

        /// <summary>
        /// Gets or sets the resources of the application.
        /// </summary>
        ResourceDictionary Resources
        {
            get; set;
        }
    }
}
