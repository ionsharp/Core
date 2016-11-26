using System.Reflection;
using WPFLocalizeExtension.Extensions;

namespace Imagin.Common.Globalization
{
    /// <summary>
    /// A utility to access global localization resources.
    /// </summary>
    public static class LocalizationProvider
    {
        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetExecutingAssembly().GetName().Name + ":Main:" + key);
        }
    }
}
