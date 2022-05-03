using System.Windows;
using Imagin.Common.Linq;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// <see cref="ThemeKey.Key"/> should not contain periods. All periods found are removed (until offending instances are fixed).
    /// </summary>
    public class ThemeKey : DynamicResourceExtension
    {
        public const string KeyFormat = "Theme/Default/{0}.xaml";

        /// <summary>
        /// Sets the key.
        /// </summary>
        public ThemeKeys ActualKey
        {
            set => ResourceKey = $"{value}";
        }

        /// <summary>
        /// Sets the key with periods removed (temporary fix).
        /// </summary>
        public string Key
        {
            set => ResourceKey = $"{Convert(value)}";
        }

        public ThemeKey() : base() { }

        public ThemeKey(string key) : this(Convert(key)) { }

        public ThemeKey(ThemeKeys key) : base() => ActualKey = key;

        static ThemeKeys Convert(string input) => input.Replace(".", "").Parse<ThemeKeys>(false);
    }
}