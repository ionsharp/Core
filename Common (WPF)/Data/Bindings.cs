using System.Windows.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class Bindings
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Binding New(string path, object source)
        {
            return new Binding(path)
            {
                Source = source
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static Binding New(string path, IValueConverter converter)
        {
            return new Binding(path)
            {
                Converter = converter
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static MultiBinding New(IMultiValueConverter converter)
        {
            return new MultiBinding()
            {
                Converter = converter
            };
        }
    }
}
