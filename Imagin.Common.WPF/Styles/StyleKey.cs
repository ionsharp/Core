using Imagin.Common.Linq;
using Imagin.Common.Markup;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// See <see cref="StyleKeys"/>.
    /// </summary>
    public class StyleKey : Uri
    {
        public const string KeyFormat = "Styles/Generic/{0}.xaml";

        public StyleKeys Key
        {
            set => RelativePath = KeyFormat.F(value);
        }

        public StyleKey() : base() => Assembly = InternalAssembly.Name;
    }
}