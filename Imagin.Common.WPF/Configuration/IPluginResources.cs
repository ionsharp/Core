using System.Collections.Generic;

namespace Imagin.Common.Configuration
{
    public interface IPluginResources
    {
        object GetValue(string key);

        IEnumerable<object> GetValues();
    }
}