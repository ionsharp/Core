using Imagin.Common.Analytics;
using Imagin.Common.Local;
using Imagin.Common.Local.Engine;
using System.Globalization;

namespace Imagin.Common.Linq
{
    public static class XLanguage
    {
        public static void Set(this Language language)
        {
            Try.Invoke(() =>
            {
                LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                LocalizeDictionary.Instance.Culture = new CultureInfo(language.GetAttribute<CultureAttribute>().Code);
            },
            e => Log.Write<Language>(e));
        }
    }
}