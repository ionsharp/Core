namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICoercable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        T Minimum
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        T Maximum
        {
            get; set;
        }
    }
}
