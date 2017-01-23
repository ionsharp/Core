namespace Imagin.Common.Config
{
    /// <summary>
    /// Specifies a control responsible for starting up an application.
    /// </summary>
    public interface IAppStarter
    {
        /// <summary>
        /// A value indicating the current progress.
        /// </summary>
        double Progress
        {
            get; set;
        }

        /// <summary>
        /// A message indicating the current status.
        /// </summary>
        string Status
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Progress"></param>
        /// <param name="Status"></param>
        void Set(double Progress, string Status);
    }
}
