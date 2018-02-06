namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICoercable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Minimum"></param>
        /// <param name="Maximum"></param>
        void SetConstraint(object Minimum, object Maximum);
    }
}
