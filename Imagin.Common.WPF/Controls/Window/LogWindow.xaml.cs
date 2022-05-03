using Imagin.Common.Analytics;
using Imagin.Common.Models;
using System.Windows;

namespace Imagin.Common.Controls
{
    public partial class LogWindow : Window
    {
        static readonly DependencyPropertyKey LogKey = DependencyProperty.RegisterReadOnly(nameof(Log), typeof(ILog), typeof(LogWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty LogProperty = LogKey.DependencyProperty;
        public ILog Log
        {
            get => (ILog)GetValue(LogProperty);
            private set => SetValue(LogKey, value);
        }

        static readonly DependencyPropertyKey PanelKey = DependencyProperty.RegisterReadOnly(nameof(Panel), typeof(LogPanel), typeof(LogWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty PanelProperty = PanelKey.DependencyProperty;
        public LogPanel Panel
        {
            get => (LogPanel)GetValue(PanelProperty);
            private set => SetValue(PanelKey, value);
        }

        LogWindow() : base() => InitializeComponent();

        public LogWindow(ILog log, LogPanel panel) : this()
        {
            Log 
                = log;
            Panel
                = panel;
        }
    }
}