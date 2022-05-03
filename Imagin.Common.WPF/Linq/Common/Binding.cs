using System.Windows.Data;

namespace Imagin.Common.Linq
{
    public static class XBinding
    {
        public static Binding Clone(this Binding input)
        {
            return new Binding()
            {
                IsAsync 
                    = input.IsAsync,
                AsyncState 
                    = input.AsyncState,
                BindingGroupName
                    = input.BindingGroupName,
                BindsDirectlyToSource 
                    = input.BindsDirectlyToSource,
                Converter 
                    = input.Converter,
                ConverterCulture
                    = input.ConverterCulture,
                ConverterParameter
                    = input.ConverterParameter,
                Delay 
                    = input.Delay,
                ElementName 
                    = input.ElementName,
                FallbackValue 
                    = input.FallbackValue,
                Mode 
                    = input.Mode,
                NotifyOnSourceUpdated 
                    = input.NotifyOnSourceUpdated,
                NotifyOnTargetUpdated 
                    = input.NotifyOnTargetUpdated,
                NotifyOnValidationError
                    = input.NotifyOnValidationError,
                Path 
                    = input.Path,
                RelativeSource 
                    = input.RelativeSource,
                Source 
                    = input.Source,
                StringFormat 
                    = input.StringFormat,
                TargetNullValue 
                    = input.TargetNullValue,
                UpdateSourceExceptionFilter 
                    = input.UpdateSourceExceptionFilter,
                UpdateSourceTrigger 
                    = input.UpdateSourceTrigger,
                ValidatesOnDataErrors 
                    = input.ValidatesOnDataErrors,
                ValidatesOnExceptions 
                    = input.ValidatesOnExceptions,
                ValidatesOnNotifyDataErrors 
                    = input.ValidatesOnNotifyDataErrors,
                XPath 
                    = input.XPath,
            };
        }
    }
}