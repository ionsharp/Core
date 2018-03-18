namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRange<T>
    {
        /// <summary>
        /// 
        /// </summary>
        T Maximum
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        T Minimum
        {
            get; set;
        }
    }
}
