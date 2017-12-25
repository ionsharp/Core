using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data 
{
    /// <summary>
    /// 
    /// </summary>
    public class FileSizeConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        FileSizeFormat GetFileSizeFormat(object Parameter)
        {
            var Result = FileSizeFormat.BinaryUsingSI;

            if (Parameter is string)
            {
                Result = (FileSizeFormat)Enum.Parse(typeof(FileSizeFormat), Parameter.ToString());
            }
            else if (Parameter is FileSizeFormat)
                Result = (FileSizeFormat)Parameter;

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var Value = default(ulong);
            if (value is long)
            {
                Value = value.To<long>().ToUInt64();
            }
            else if (value is ulong)
                Value = value.To<ulong>();

            return Value.ToFileSize(GetFileSizeFormat(parameter), 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
