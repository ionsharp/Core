using System;
using System.Windows;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// An independent library used to extend functionality of an application.
    /// </summary>
    public interface IPlugin
    {
        event EventHandler<EventArgs> Enabled;

        event EventHandler<EventArgs> Disabled;

        AssemblyContext AssemblyContext { get; set; }

        string Author { get; }

        string Description { get; }

        string Icon { get; }

        bool IsEnabled { get; set; }

        string Name
        {
            get;
        }

        string Uri { get; }

        Version Version { get; }

        void OnEnabled();

        void OnDisabled();
    }
}