namespace Imagin.Common.Local.Providers
{
    /// <summary>
    /// An abstract class for key identification.
    /// </summary>
    public abstract class FullyQualifiedResourceKeyBase
    {
        /// <summary>
        /// Implicit string operator.
        /// </summary>
        /// <param name="fullyQualifiedResourceKey">The object.</param>
        /// <returns>The joined version of the assembly, dictionary and key.</returns>
        public static implicit operator string(FullyQualifiedResourceKeyBase fullyQualifiedResourceKey)
        {
            return fullyQualifiedResourceKey?.ToString();
        }
    }
}