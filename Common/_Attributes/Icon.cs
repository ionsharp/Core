using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field)]
    public class IconAttribute : Attribute
    {
        readonly string uri;
        /// <summary>
        /// 
        /// </summary>
        public string Uri
        {
            get => uri;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Uri"></param>
        public IconAttribute(string Uri) : base()
        {
            uri = Uri;
        }
    }
}
