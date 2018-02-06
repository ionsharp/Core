using System;

namespace Imagin.Common
{
    /// <summary>
    /// A generic alternative for <see langword="System.ComponentModel.BrowsableAttribute"/>, which isn't available in some frameworks.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BrowsableAttribute : Attribute
    {
        readonly bool browsable;
        /// <summary>
        /// 
        /// </summary>
        public bool Browsable
        {
            get => browsable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Browsable"></param>
        public BrowsableAttribute(bool Browsable) : base()
        {
            browsable = Browsable;
        }
    }
}
