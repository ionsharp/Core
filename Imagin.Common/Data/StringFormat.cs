namespace Imagin.Common.Data
{
    /// <summary>
    /// Provides enumerator for strings 
    /// of various representations.
    /// </summary>
    public enum StringFormat
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
        /// <summary>
        /// Specifies a series of tokens.
        /// </summary>
        Tokens,
    }
}
