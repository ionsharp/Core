using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, bool canExecute) : base(execute, canExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute)
        {
        }
    }
}