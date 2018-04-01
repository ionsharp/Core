using Imagin.Common.Linq;
using System;
using System.Windows.Data;

namespace Imagin.Common.Data
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
            Converter = Imagin.Common.Converters.Converter<object, object>.New
            (
                input => input?.GetType().GetEnumValues(Appearance.Visible), 
                input => throw new NotSupportedException()
            );
            Mode = BindingMode.OneTime;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }
    }
}