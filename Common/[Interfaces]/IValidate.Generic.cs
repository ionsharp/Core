namespace Imagin.Common
{
    /// <summary>
    /// Specifies a handler that validates a series of arguments and produces a <see cref="System.Boolean"/> result.
    /// </summary>
    public interface IValidate<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        bool Validate(params T[] Arguments);
    }
}
