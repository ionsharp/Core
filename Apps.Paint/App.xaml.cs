using Imagin.Common;
using Imagin.Common.Configuration;
using System;
using System.Collections.Generic;

namespace Imagin.Apps.Paint
{
    public partial class App : SingleApplication
    {
        public const string DefaultName
            = "Imagin.Apps.Paint";

        public const string ImagePath
            = "pack://application:,,,/" + DefaultName + ";component/Images/";

        public override ApplicationProperties Properties => new ApplicationProperties<MainWindow, MainViewModel, Options>();

        [STAThread]
        public static void Main(params string[] Arguments)
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(nameof(Paint)))
            {
                var App = new App();
                App.InitializeComponent();
                App.Run();
                SingleInstance<App>.Cleanup();
            }
        }

        protected override void OnLoaded(IList<string> arguments)
        {
            base.OnLoaded(arguments);
            _ = Get.Current<MainViewModel>().OpenAsync(arguments);
        }

        public override void OnReopened(IList<string> arguments)
        {
            base.OnReopened(arguments);
            _ = Get.Current<MainViewModel>().OpenAsync(arguments);
        }
    }
}