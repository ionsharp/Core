using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class UriBox : ParseBox<Uri>
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty KindProperty = DependencyProperty.Register("Kind", typeof(UriKind), typeof(UpDown), new FrameworkPropertyMetadata(default(UriKind), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public UriKind Kind
        {
            get
            {
                return (UriKind)GetValue(KindProperty);
            }
            set
            {
                SetValue(KindProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UriBox() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override Uri GetValue(string value)
        {
            return value.ToUri(Kind);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string ToString(Uri value)
        {
            return value.ToString();
        }
    }
}
