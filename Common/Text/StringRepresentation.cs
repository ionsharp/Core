namespace Imagin.Common.Text
{
    /// <summary>
    /// Provides enumerator for strings 
    /// of various representations.
    /// </summary>
    /// <remarks>
    /// 'Unspecified' refers to any kind 
    /// not specified in enumeration
    /// and should be the default value.
    /// </remarks>
    public enum StringRepresentation
    {
        Unspecified = 0,
        Regular = 1,
        FileSystemPath = 2,
        Multiline = 3,
        Password = 4
    }
}
