using Imagin.Common.Data.Converters;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class MathBinding : Binding
    {
        /// <summary>
        /// The type of math operation to perform (note, the operation is unknown unless otherwise specified).
        /// </summary>
        public MathOperation Type { get; set; } = MathOperation.Unknown;

        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; } = MathOperation.Unknown;

        protected virtual void OnInitialized()
        {
            Converter = new MathConverter(Type, Value);
            ConverterParameter = Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        public MathBinding(string Path) : base(Path)
        {
            Mode = BindingMode.OneWay;
            OnInitialized();
        }
    }
}
