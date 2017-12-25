using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        void AssertParameter()
        {
            throw new ArgumentException("A value of 'Normal' or 'Inverted' is expected.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            var Value = default(bool);
            var Parameter = parameter?.ToString();

            //Get the value
            if (value is bool)
                Value = (bool)value;

            //Get initial result
            var Result = Value.ToVisibility();

            //Get whether or not to invert
            if (Parameter != null)
            {
                if (Parameter == "Normal")
                {
                    //Do nothing!
                }
                else if (Parameter == "Inverted")
                {
                    //Invert as necessary
                    Result = Result.Invert();
                }
                else AssertParameter();
            }

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
        public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var Value = default(Visibility);
            var Parameter = parameter?.ToString();

            //Get the value
            if (value is Visibility)
                Value = (Visibility)value;

            //Get initial result
            var Result = Value.ToBoolean();

            //Get whether or not to invert
            if (Parameter != null)
            {
                if (Parameter == "Normal")
                {
                    //Do nothing!
                }
                else if (Parameter == "Inverted")
                {
                    //Invert as necessary
                    Result = Result.Invert();
                }
                else AssertParameter();
            }

            return Result;
        }
    }
}
