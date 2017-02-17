using System.Collections.Generic;
using Imagin.Common.Extensions;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an object capable of querying system objects.
    /// </summary>
    public interface ISystemProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path">The path to query.</param>
        /// <param name="Source">A source used to make queries.</param>
        /// <returns>A list of system object paths.</returns>
        IEnumerable<string> Query(string Path, object Source = null);
    }

    /// <summary>
    /// Defines base functionality for an <see cref="ISystemProvider"/>.
    /// </summary>
    public abstract class SystemProvider : ISystemProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public abstract IEnumerable<string> Query(string Path, object Source = null);
    }

    /// <summary>
    /// Defines functionality to query a local system.
    /// </summary>
    public class LocalSystemProvider : SystemProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public override IEnumerable<string> Query(string Path, object Source = null)
        {
            if (Path.IsNullOrEmpty())
            {
                foreach (var i in System.IO.DriveInfo.GetDrives())
                    yield return i.Name;
            }
            else
            {
                if (System.IO.Directory.Exists(Path))
                {
                    foreach (var i in System.IO.Directory.EnumerateFileSystemEntries(Path))
                        yield return i;
                }
            }
        }
    }
}
