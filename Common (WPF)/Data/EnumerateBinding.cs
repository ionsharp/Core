using Imagin.Common.Data;
using System.Windows.Data;

namespace Imagin.Common.Markup
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnumerateBinding : Binding
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public EnumerateBinding(string path) : base(path)
        {
            Converter = new EnumToCollectionConverter();
            Mode = BindingMode.OneTime;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }
    }
}
