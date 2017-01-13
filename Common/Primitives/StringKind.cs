namespace Imagin.Common.Primitives
{
    /// <summary>
    /// Provides enumerator for strings 
    /// of various representations.
    /// </summary>
    public enum StringKind
    {
        /// <summary>
        /// Unspecified string (or default).
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// Specifies a regular string.
        /// </summary>
        Regular = 1,
        /// <summary>
        /// Specifies a file path.
        /// </summary>
        FilePath = 2,
        /// <summary>
        /// Specifies a folder path.
        /// </summary>
        FolderPath = 3,
        /// <summary>
        /// Specifies a multiline string.
        /// </summary>
        Multiline = 4,
        /// <summary>
        /// Specifies a password string.
        /// </summary>
        Password = 5
    }
}
