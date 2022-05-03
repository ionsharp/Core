using System;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies a type of <see cref="Vector"/> (<see cref="Column"/> corresponds to <see cref="Matrix"/> ∈ [m, 1]; <see cref="Row"/> corresponds to <see cref="Matrix"/> ∈ [1, n]).
    /// </summary>
    [Serializable]
    public enum VectorType
    {
        /// <summary>
        /// Corresponds to <see cref="Matrix"/> ∈ [m, 1].
        /// </summary>
        Column,
        /// <summary>
        /// Corresponds to <see cref="Matrix"/> ∈ [1, n].
        /// </summary>
        Row
    }
}
