using System;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly Action<T> execute = null;

        /// <summary>
        /// 
        /// </summary>
        protected readonly Predicate<T> canExecute = null;

        /// <summary>
        /// 
        /// </summary>
        protected readonly Func<T, Task<T>> canExecuteTask = null;

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<T> execute, bool canExecute) : this(execute, new Predicate<T>(x => canExecute))
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute((T)parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}
