using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class DialogPresenter : Presenter<Window>
    {
        static readonly DependencyPropertyKey CurrentDialogKey = DependencyProperty.RegisterReadOnly(nameof(CurrentDialog), typeof(DialogReference), typeof(DialogPresenter), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty CurrentDialogProperty = CurrentDialogKey.DependencyProperty;
        public DialogReference CurrentDialog
        {
            get => (DialogReference)GetValue(CurrentDialogProperty);
            set => SetValue(CurrentDialogKey, value);
        }

        static readonly DependencyPropertyKey IsShowingKey = DependencyProperty.RegisterReadOnly(nameof(IsShowing), typeof(bool), typeof(DialogPresenter), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsShowingProperty = IsShowingKey.DependencyProperty;
        public bool IsShowing
        {
            get => (bool)GetValue(IsShowingProperty);
            set => SetValue(IsShowingKey, value);
        }

        public DialogPresenter() : base() { }

        //...

        internal void Close(int input)
        {
            var dialogs = XWindow.GetDialogs(Control);

            var last = dialogs.Pop();
            last.Result = input;

            CurrentDialog
                = dialogs.Count > 0 && dialogs.Peek() is DialogReference i 
                ? i : null;

            IsShowing = false;
        }

        internal void Show(DialogReference input)
        {
            XWindow.GetDialogs(Control).Push(input);
            CurrentDialog = input;
            IsShowing = true;
        }

        ICommand closeCommand;
        public ICommand CloseCommand => closeCommand
            ??= new RelayCommand<int>(i => Close(i));
    }
}