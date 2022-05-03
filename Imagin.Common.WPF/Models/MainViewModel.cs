using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    public abstract class MainViewModel<T> : ViewModel<T>, IMainViewModel where T : MainWindow
    {
        bool handleExit = false;

        //...

        [Hidden]
        public virtual string Title => XAssembly.Title();

        Window IMainViewModel.View
        {
            get => View;
            set => View = value.As<T>();
        }
        [Hidden]
        public override T View
        {
            get => base.View;
            set
            {
                base.View = value;
                if (View != null)
                {
                    View.DataContext = this;

                    View.Closed += OnWindowClosed;
                    View.Closing += OnWindowClosing;
                }
            }
        }

        //...

        public MainViewModel() : base(null)
        {
            Get.Register(GetType(), this);
            Get.Where<MainViewOptions>().PropertyChanged += OnOptionsChanged;
        }

        //...

        void OnOptionsChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MainViewOptions.WindowShowInTaskBar):
                    if (!Get.Where<MainViewOptions>().WindowShowInTaskBar)
                        Show();
                    
                    break;
            }
        }

        void OnWindowClosed(object sender, System.EventArgs e) => OnWindowClosed();

        void OnWindowClosing(object sender, CancelEventArgs e) => OnWindowClosing(e);

        //...

        void Hide()
        {
            View.Hide();
            View.ShowInTaskbar = false;
        }

        void Show()
        {
            View.Show();
            View.ShowInTaskbar = true;
        }

        //...

        protected virtual void OnWindowClosed() { }

        protected virtual void OnWindowClosing(CancelEventArgs e)
        {
            if (Get.Where<MainViewOptions>().WindowShowInTaskBar)
            {
                if (!handleExit)
                {
                    e.Cancel = true;
                    Hide();
                }
            }
        }

        //...

        ICommand deleteFileCommand;
        [Hidden]
        public ICommand DeleteFileCommand => deleteFileCommand ??= new RelayCommand<string>(i =>
        {
            var result = Dialog.Show("Delete", $"Are you sure you want to delete '{i}'?", DialogImage.Warning, Buttons.YesNo);
            if (result == 0)
                Try.Invoke(() => Computer.Recycle(i, RecycleOption.DeletePermanently));
        },
        i => File.Long.Exists(i));

        ICommand recycleFileCommand;
        [Hidden]
        public ICommand RecycleFileCommand => recycleFileCommand ??= new RelayCommand<string>(i =>
        {
            var result = Dialog.Show("Recycle", $"Are you sure you want to recycle '{i}'?", DialogImage.Warning, Buttons.YesNo);
            if (result == 0)
                Try.Invoke(() => Computer.Recycle(i, RecycleOption.SendToRecycleBin));
        },
        i => File.Long.Exists(i));

        ICommand hideShowCommand;
        [Hidden]
        public ICommand HideShowCommand => hideShowCommand ??= new RelayCommand(() =>
        {
            if (View.IsVisible)
                Hide();

            else Show();
        });

        ICommand exitCommand;
        [Hidden]
        public ICommand ExitCommand => exitCommand ??= new RelayCommand(() => View.Close(), () => true);

        ICommand forceExitCommand;
        [Hidden]
        public ICommand ForceExitCommand => forceExitCommand ??= new RelayCommand(() =>
        {
            handleExit = true;
            View.Close();
        },
        () => true);

        ICommand showAboutWindowCommand;
        [Hidden]
        public ICommand ShowAboutWindowCommand => showAboutWindowCommand ??= new RelayCommand(() => new AboutWindow().ShowDialog(), () => true);

        ICommand showOptionsWindowCommand;
        [Hidden]
        public ICommand ShowOptionsWindowCommand => showOptionsWindowCommand ??= new RelayCommand(() => new OptionsWindow().Show(), () => true);
    }
}