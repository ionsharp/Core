namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DescriptionAttributeConverter : AttributeConverterBase<DescriptionAttribute>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        protected override object GetValue(DescriptionAttribute Attribute)
        {
            return Attribute.Description;
        }
    }
}
