namespace Imagin.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static T As<T>(this object ToCast)
        {
            if (ToCast is T)
                return (T)ToCast;
            return default(T);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static bool Is<T>(this object ToEvaluate)
        {
            return ToEvaluate is T;
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static dynamic ToDynamic(this object ToConvert)
        {
            return (dynamic)ToConvert;
        }
    }
}
