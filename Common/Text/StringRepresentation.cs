namespace Imagin.Common.Text
{
    /// <summary>
    /// Provides enumerator for strings 
    /// of various representations.
    /// </summary>
    public enum StringRepresentation
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
        /// Specifies a file system path.
        /// </summary>
        FileSystemPath = 2,
        /// <summary>
        /// Specifies a multiline string.
        /// </summary>
        Multiline = 3,
        /// <summary>
        /// Specifies a password string.
        /// </summary>
        Password = 4
    }
}
