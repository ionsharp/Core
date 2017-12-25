using System;
using System.IO;
using System.Windows;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Returns a Uri that represents a resource.
        /// </summary>
        public static Stream GetResourceStream(this Uri ResourceUri)
        {
            return Application.GetResourceStream(ResourceUri).Stream;
        }
    }
}
