using Imagin.Common.Data;
using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnumerateExtension : MarkupExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public Type Type
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public EnumerateExtension(Type type) : base()
        {
            Type = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new EnumToCollectionConverter().Convert(Type);
        }
    }
}
