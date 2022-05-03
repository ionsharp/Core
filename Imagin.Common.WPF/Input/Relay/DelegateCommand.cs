using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Imagin.Common.Input
{
    public sealed class DelegateCommand : ICommand
    {
        private List<WeakReference> _canExcuteChangeHandlers;

        private readonly Func<object, Task> _excutedMethod;
        private readonly Func<object, bool> _canExcuteMethod;

        public DelegateCommand(Action<object> excuteMethod, Func<object, bool> canExecuteMethod)
        {
            if (excuteMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException();

            _excutedMethod = (arg) =>
            {
                excuteMethod(arg);
                return Task.Delay(0);
            };
            _canExcuteMethod = canExecuteMethod;

        }

        public DelegateCommand(Func<object, Task> excuteMethod, Func<object, bool> canExecuteMethod)
        {
            if (excuteMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException();

            _excutedMethod = excuteMethod;
            _canExcuteMethod = canExecuteMethod;

        }

        private void OnCanExecuteChanged()
        {
            WeakEventHandlerManager.CallWeakReferenecHandlers(this, _canExcuteChangeHandlers);
        }

        public void RaiseCanExecuteChanger()
        {
            OnCanExecuteChanged();
        }

        async void ICommand.Execute(object parameter)
        {
            await Execute(parameter);
        }

        private async Task Execute(object parameter)
        {
            await _excutedMethod(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        private bool CanExecute(object parameter)
        {
            return _canExcuteMethod == null || _canExcuteMethod(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExcuteChangeHandlers, value, 2);
            }
            remove
            {
                WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExcuteChangeHandlers, value);
            }
        }
    }
}
