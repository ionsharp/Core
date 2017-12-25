using Imagin.Common.Input;
using System;

namespace Imagin.Common.Mvvm
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelBase : BindableObject, IInitializable
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executeMethod"></param>
        /// <returns></returns>
        protected DelegateCommand CreateCommand(Action<object> executeMethod)
        {
            return CreateCommand(executeMethod, (o) => true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executeMethod"></param>
        /// <param name="canExecuteMethod"></param>
        /// <returns></returns>
        protected DelegateCommand CreateCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            return new DelegateCommand(executeMethod, canExecuteMethod);
        }
    }
}
