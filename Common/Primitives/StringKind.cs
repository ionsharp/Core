namespace Imagin.Common.Primitives
{
    /// <summary>
    /// Provides enumerator for strings 
    /// of various representations.
    /// </summary>
    public enum StringKind
    {
        /// <summary>
        /// Specifies a regular string.
        /// </summary>
        Default,
        /// <summary>
        /// Specifies a file path.
        /// </summary>
        FilePath,
        /// <summary>
        /// Specifies a folder path.
        /// </summary>
        FolderPath,
        /// <summary>
        /// Specifies a multiline string.
        /// </summary>
        Multiline,
        /// <summary>
        /// Specifies a password string.
        /// </summary>
        Password,
    }
}
