using System;

namespace Imagin.Common.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field)]
    [Serializable]
    public class IconAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Uri
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Uri"></param>
        public IconAttribute(string Uri)
        {
            this.Uri = Uri;
        }
    }
}
