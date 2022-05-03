using Imagin.Common.Converters;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class VisibilityBinding : LocalBinding
    {
        public bool Invert
        {
            set => ConverterParameter = value ? 1 : 0;
        }

        public VisibilityBinding() : this(".") { }

        public VisibilityBinding(string path) : this(path, 0) { }

        public VisibilityBinding(string path, int converterParameter) : base(path)
        {
            Converter = new ComplexConverter<object, object>(i =>
            {
                if (i.ActualValue is bool || i.ActualValue is Visibility)
                    return BooleanToVisibilityConverter.Default.Convert(i.ActualValue, null, i.ActualParameter, null);

                if (i.ActualValue is int)
                    return Int32ToVisibilityConverter.Default.Convert(i.ActualValue, null, i.ActualParameter, null);

                if (i.ActualValue is string)
                    return StringToVisibilityConverter.Default.Convert(i.ActualValue, null, i.ActualParameter, null);

                return ObjectToVisibilityConverter.Default.Convert(i.ActualValue, null, i.ActualParameter, null);
            }, i =>
            {
                if (i.ActualValue is bool || i.ActualValue is Visibility)
                    return BooleanToVisibilityConverter.Default.ConvertBack(i.ActualValue, null, i.ActualParameter, null);

                if (i.ActualValue is int)
                    return Int32ToVisibilityConverter.Default.ConvertBack(i.ActualValue, null, i.ActualParameter, null);

                if (i.ActualValue is string)
                    return StringToVisibilityConverter.Default.ConvertBack(i.ActualValue, null, i.ActualParameter, null);

                return ObjectToVisibilityConverter.Default.ConvertBack(i.ActualValue, null, i.ActualParameter, null);
            });
            ConverterParameter = converterParameter;
        }
    }

    public class VisibilityMultiBinding : MultiBinding
    {
        public VisibilityMultiBinding() : base()
        {
            Converter
                = BooleanToVisibilityMultiConverter.Default;
            Mode 
                = BindingMode.OneWay;
        }
    }
}