using Imagin.Common.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalPathValidateHandler : PathValidateHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        protected sealed override bool FileExists(string Path)
        {
            return Path.FileExists();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        protected sealed override bool FolderExists(string Path)
        {
            return Path.DirectoryExists();
        }
    }
}
