using System;

namespace Imagin.Common.IO
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum NotifyFilters
    {
        /// <summary>
        /// 
        /// </summary>
        Attributes,
        /// <summary>
        /// 
        /// </summary>
        CreationTime,
        /// <summary>
        /// 
        /// </summary>
        DirectoryName,
        /// <summary>
        /// 
        /// </summary>
        FileName,
        /// <summary>
        /// 
        /// </summary>
        LastAccess,
        /// <summary>
        /// 
        /// </summary>
        LastWrite,
        /// <summary>
        /// 
        /// </summary>
        Security,
        /// <summary>
        /// 
        /// </summary>
        Size,
        /// <summary>
        /// 
        /// </summary>
        All = Attributes | CreationTime | DirectoryName | FileName | LastAccess | LastWrite | Security | Size
    }
}
