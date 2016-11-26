using System;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// An indepedent library used to extend functionality in an application.
    /// </summary>
    public interface IPlugin
    {
        event EventHandler<EventArgs> Enabled;

        /// <summary>
        /// The individual who (or organization that) developed the plugin.
        /// </summary>
        string Author
        {
            get; 
        }

        /// <summary>
        /// A short description explaining what the plugin does.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Uri for an icon resource.
        /// </summary>
        string Icon
        {
            get;
        }

        /// <summary>
        /// Whether or not the plugin is enabled.
        /// </summary>
        bool IsEnabled
        {
            get; set;
        }

        /// <summary>
        /// The name of the plugin.
        /// </summary>
        string Name
        {
            get; 
        }

        /// <summary>
        /// Website where the plugin is published.
        /// </summary>
        string Uri
        {
            get; 
        }

        /// <summary>
        /// Plugin version.
        /// </summary>
        Version Version
        {
            get; 
        }

        /// <summary>
        /// Enable the plugin.
        /// </summary>
        void Enable();

        /// <summary>
        /// Disable the plugin.
        /// </summary>
        void Disable();
    }
}
