using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum DayOfWeek
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Sunday = 1,
        /// <summary>
        /// 
        /// </summary>
        Monday = 2,
        /// <summary>
        /// 
        /// </summary>
        Tuesday = 4,
        /// <summary>
        /// 
        /// </summary>
        Wednesday = 8,
        /// <summary>
        /// 
        /// </summary>
        Thursday = 16,
        /// <summary>
        /// 
        /// </summary>
        Friday = 32,
        /// <summary>
        /// 
        /// </summary>
        Saturday = 64,
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        All = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday
    }
}
