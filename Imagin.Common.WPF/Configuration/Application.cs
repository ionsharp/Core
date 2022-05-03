using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using Imagin.Common.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Imagin.Common.Configuration
{
    public abstract class BaseApplication : Application, IApplication
    {
        public static readonly ResourceKey<SplashWindow> SplashWindowStyleKey = new();

        //...

        public event UnhandledExceptionEventHandler ExceptionUnhandled;

        public event EventHandler<EventArgs> Loaded;

        //...

        readonly StartQueue StartTasks = new();

        //...

        new public abstract ApplicationProperties Properties { get; }

        //...

        public LogWriter Log 
            { get; private set; }

        public IMainViewModel MainViewModel 
            { get; private set; }

        new public MainWindow MainWindow 
            { get => (MainWindow)base.MainWindow; set => base.MainWindow = value; }

        public NotificationWriter Notifications 
            { get; private set; }

        public IMainViewOptions Options 
            { get; private set; }

        public Plugins Plugins
            { get; private set; } = null;

        public Updater Updater
            { get; private set; } = null;

        //...

        public BaseApplication() : base()
        {
            Get.Register(GetType(), this);

            //...

            AppDomain.CurrentDomain.UnhandledException 
                += OnExceptionUnhandled;
            DispatcherUnhandledException 
                += OnExceptionUnhandled;
            StartTasks.Completed
                += OnStartTasksCompleted;
            TaskScheduler.UnobservedTaskException 
                += OnExceptionUnhandled;

            //...

            Log
                = new LogWriter(Properties.FolderPath, LogWriter.DefaultLimit);

            Notifications
                = new NotificationWriter(Properties.FolderPath, NotificationWriter.DefaultLimit);

            Plugins
                = new Plugins(Properties.GetFolderPath("Plugins"));

            Updater
                = new Updater(null, null, null, null);

            //...

            Get.Create<ApplicationResources>(this)
                .LoadTheme(DefaultThemes.Light);

            //...

            StartTasks.Add(false,"Log", () => Log.Load());
            StartTasks.Add(false,"Notifications", () => Notifications.Load());
            StartTasks.Add(false,"Plugins", () => Plugins.Refresh());
            StartTasks.Add(true, "Options", () => 
            { 
                BinarySerializer.Deserialize(Properties.FilePath, out MainViewOptions options);
                Options = options ?? Properties.MainViewOptions.Create<MainViewOptions>();
            });
            StartTasks.Add(true, "View", () => MainViewModel = Properties.MainViewModel.Create<IMainViewModel>());
        }

        //...

        void OnMainWindowClosingFinal(object sender, CancelEventArgs e)
        {
            Error error = null;
            Action action = new(() =>
            {
                Try.Invoke(() =>
                {
                    Notifications
                        .Save();
                    Options
                        .Save();
                }, e =>
                {
                    Log.Write<BaseApplication>(e);
                    error = new(e);
                });
                Try.Invoke(() => Log.Save(), e =>
                {
                    Log.Write<BaseApplication>(e);
                    error = new(e);
                });
            });

            if (Get.Where<IMainViewOptions>().SaveWithDialog)
                new LoadWindow(1.Seconds(), null, action).ShowDialog();

            else action();

            if (error != null)
            {
                var result = Dialog.Show(XAssembly.Product(), "An error was logged while saving. Close anyway?", DialogImage.Error, Buttons.YesNo);
                if (result == 1)
                    e.Cancel = true;
            }
        }

        async void OnStartTasksCompleted(object sender, EventArgs e)
        {
            OnLoaded(Environment.GetCommandLineArgs().Skip(1).ToArray());
            await StartTasks.SplashWindow.FadeOut();

            MainWindow = Properties.MainView.Create<MainWindow>();
            MainWindow.ClosingFinal += OnMainWindowClosingFinal;
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainWindow.Show();

            StartTasks.SplashWindow.Close();
        }

        //...

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var window = new SplashWindow();
            window.SetResourceReference(SplashWindow.StyleProperty, SplashWindowStyleKey);
            window.Shown += OnSplashWindowShown;
            window.Show();
        }

        void OnSplashWindowShown(object sender, EventArgs e)
        {
            if (sender is SplashWindow window)
            {
                window.Shown -= OnSplashWindowShown;
                _ = StartTasks.Invoke(window);
            }
        }

        //...

        protected virtual void OnLoaded(IList<string> arguments) => Loaded?.Invoke(this, EventArgs.Empty);

        //...

        void OnExceptionUnhandled(object sender, System.UnhandledExceptionEventArgs e)
        {
            OnExceptionUnhandled(UnhandledExceptions.AppDomain, e.ExceptionObject as Exception);
        }

        void OnExceptionUnhandled(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG  
            e.Handled = false;
#else
            e.Handled = true;
#endif     
            OnExceptionUnhandled(UnhandledExceptions.Dispatcher, e.Exception);
        }

        void OnExceptionUnhandled(object sender, UnobservedTaskExceptionEventArgs e)
        {
#if DEBUG  

#else
            e.SetObserved();
#endif     
            OnExceptionUnhandled(UnhandledExceptions.TaskScheduler, e.Exception);
        }

        //...

        protected virtual void OnExceptionUnhandled(UnhandledExceptions type, Exception e)
        {
            var error = new Error(e);
            Log.Write<BaseApplication>(error, TraceLevel.High);
            ExceptionUnhandled?.Invoke(this, new UnhandledExceptionEventArgs(type, error));
        }
    }
}