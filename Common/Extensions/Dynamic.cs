namespace Imagin.Common.Extensions
{
    public static class DynamicExtensions
    {
        public static bool PropertyExists(dynamic ToEvaluate, string PropertyName)
        {
            return ToEvaluate.GetType().GetProperty(PropertyName) != null;
        }
    }
}
