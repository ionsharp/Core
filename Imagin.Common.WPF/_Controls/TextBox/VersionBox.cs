using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class VersionBox : ParseBox<Version>
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DelimiterProperty = DependencyProperty.Register("Delimiter", typeof(char), typeof(VersionBox), new FrameworkPropertyMetadata('.', FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public char Delimiter
        {
            get
            {
                return (char)GetValue(DelimiterProperty);
            }
            set
            {
                SetValue(DelimiterProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public VersionBox() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override Version GetValue(string value)
        {
            return value.ToVersion(Delimiter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string ToString(Version value)
        {
            return value?.ToString();
        }
    }
}
