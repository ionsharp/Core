using System;
using System.Windows;

namespace Imagin.Common.Config
{
    /// <summary>
    /// An independent library used to extend functionality of an application.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Occurs when the plugin is enabled.
        /// </summary>
        event EventHandler<EventArgs> Enabled;

        /// <summary>
        /// Occurs when the plugin is disabled.
        /// </summary>
        event EventHandler<EventArgs> Disabled;

        /// <summary>
        /// Gets the name of the individual who, or organization that, developed the plugin.
        /// </summary>
        string Author
        {
            get;
        }

        /// <summary>
        /// Gets short description explaining what the plugin does.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Gets <see cref="System.Uri"/> that points to an icon resource.
        /// </summary>
        string Icon
        {
            get;
        }

        /// <summary>
        /// Gets or sets whether or not the plugin is enabled.
        /// </summary>
        bool IsEnabled
        {
            get; set;
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        string Name
        {
            get; 
        }

        /// <summary>
        /// Gets resources used just for the plugin.
        /// </summary>
        ResourceDictionary Resources
        {
            get;
        }

        /// <summary>
        /// Gets a web address where the plugin might be published.
        /// </summary>
        string Uri
        {
            get; 
        }

        /// <summary>
        /// Gets the version of the plugin.
        /// </summary>
        Version Version
        {
            get; 
        }

        /// <summary>
        /// Occurs when the plugin is enabled.
        /// </summary>
        void OnEnabled();

        /// <summary>
        /// Occurs when the plugin is disabled.
        /// </summary>
        void OnDisabled();
    }
}
