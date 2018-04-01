using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Imagin.Common.IO
{
    /// <summary>
    /// Defines functionality to query a local system.
    /// </summary>
    public class LocalSystemObjectProvider : SystemObjectProvider
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
                var Drives = Enumerable.Empty<DriveInfo>();

                try
                {
                    Drives = DriveInfo.GetDrives();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                foreach (var i in Drives)
                    yield return i.Name;
            }
            else if (Directory.Exists(Path))
            {
                var Objects = Enumerable.Empty<string>();

                try
                {
                    Objects = Directory.EnumerateFileSystemEntries(Path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                foreach (var i in Objects)
                    yield return i;
            }
        }
    }
}
