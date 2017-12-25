namespace Imagin.Common.Data
{
    public class KeyAttributeConverter : AttributeConverterBase<KeyAttribute>
    {
        protected override object GetValue(KeyAttribute Attribute)
        {
            return Attribute.Key;
        }
    }
}
