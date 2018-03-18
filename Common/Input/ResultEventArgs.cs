using System;
using System.Collections.Generic;
using System.Text;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class ResultEventArgs : EventArgs
    {
        readonly object _result;
        /// <summary>
        /// 
        /// </summary>
        public object Result => _result;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public ResultEventArgs(object result) : base() => _result = result;
    }
}
