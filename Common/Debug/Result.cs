using System;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents the result of something.
    /// </summary>
    public class Result : AbstractObject
    {
        public object Data
        {
            get; set;
        }

        public Result() : base()
        {
        }

        public Result(object data) : base()
        {
            Data = data;
        }

        /// <summary>
        /// If result is success, true; else, false.
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator bool(Result a)
        {
            return a is Success;
        }
    }
}
