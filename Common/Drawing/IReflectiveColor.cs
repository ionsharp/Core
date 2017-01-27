namespace Imagin.Common.Drawing
{
    /// <summary>
    /// Specifies a color that is reflective, or defined with both an <see cref="Imagin.Common.Drawing.Illuminant"/> and <see cref="Imagin.Common.Drawing.ObserverAngle"/>.
    /// </summary>
    public interface IReflectiveColor
    {
        /// <summary>
        /// 
        /// </summary>
        Illuminant Illuminant
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        ObserverAngle Observer
        {
            get;
        }
    }
}
