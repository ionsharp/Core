using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A <see cref="ContentControl"/> that wraps another control of type, <see cref="{TContent}"/>. 
    /// </summary>
    /// <typeparam name="TContent"></typeparam>
    public class ContentControl<TContent> : ContentControl
    {
        /// <summary>
        /// 
        /// </summary>
        public TContent Source
        {
            get
            {
                return Content.As<TContent>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual TContent GetInstance()
        {
            try
            {
                return (TContent)Activator.CreateInstance(typeof(TContent));
            }
            catch
            {
                return default(TContent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ContentControl() : base()
        {
            Content = GetInstance();
            HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
        }
    }
}
