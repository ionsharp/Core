using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ThemeDictionary : StyleDictionary
    {
        const string sourceUri = "pack://application:,,,/{0};component/Color/{1}.xaml";

        Theme _theme = Theme.Light;
        /// <summary>
        /// 
        /// </summary>
        public Theme Theme
        {
            get => _theme;
            set
            {
                _theme = value;
                OnThemeChanged(_theme);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ThemeDictionary() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected override void OnAssemblyChanged(string Value) => Source = new Uri(sourceUri.F(Value, _theme), UriKind.Absolute);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnThemeChanged(Theme Value) => Source = new Uri(sourceUri.F(Assembly, Value), UriKind.Absolute);
    }
}
