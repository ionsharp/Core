using System;

namespace Imagin.Common.Colors
{
    [Flags]
    [Serializable]
    public enum Components
    {
        /// <summary>
        /// The first <see cref="Models.Component"/> of a <see cref="Models.Model"/>.
        /// </summary>
        A,
        /// <summary>
        /// The second <see cref="Models.Component"/> of a <see cref="Models.Model"/>.
        /// </summary>
        B,
        /// <summary>
        /// The third <see cref="Models.Component"/> of a <see cref="Models.Model"/>.
        /// </summary>
        C,
    }
}