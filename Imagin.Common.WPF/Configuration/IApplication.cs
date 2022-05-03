using System;
using System.Windows;

namespace Imagin.Common.Configuration
{
    public interface IApplication
    {
        event EventHandler<EventArgs> Loaded;

        ApplicationProperties Properties { get; }

        ResourceDictionary Resources { get; }
    }
}