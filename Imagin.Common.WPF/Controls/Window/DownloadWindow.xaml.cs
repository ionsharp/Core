using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Windows;

namespace Imagin.Common.Controls
{
    public partial class DownloadWindow : Window
    {
        public static readonly ReferenceKey<DownloadControl> DownloadControlKey = new();

        public static readonly DependencyProperty AutoCloseProperty = DependencyProperty.Register(nameof(AutoClose), typeof(bool), typeof(DownloadWindow), new FrameworkPropertyMetadata(false));
        public bool AutoClose
        {
            get => (bool)GetValue(AutoCloseProperty);
            set => SetValue(AutoCloseProperty, value);
        }

        public static readonly DependencyProperty AutoStartProperty = DependencyProperty.Register(nameof(AutoStart), typeof(bool), typeof(DownloadWindow), new FrameworkPropertyMetadata(false));
        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
        }

        static readonly DependencyPropertyKey DestinationKey = DependencyProperty.RegisterReadOnly(nameof(Destination), typeof(string), typeof(DownloadWindow), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty DestinationProperty = DestinationKey.DependencyProperty;
        public string Destination
        {
            get => (string)GetValue(DestinationProperty);
            private set => SetValue(DestinationKey, value);
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(DownloadWindow), new FrameworkPropertyMetadata(null));
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly DependencyProperty MessageTemplateProperty = DependencyProperty.Register(nameof(MessageTemplate), typeof(DataTemplate), typeof(DownloadWindow), new FrameworkPropertyMetadata(null));
        public DataTemplate MessageTemplate
        {
            get => (DataTemplate)GetValue(MessageTemplateProperty);
            set => SetValue(MessageTemplateProperty, value);
        }

        static readonly DependencyPropertyKey SourceKey = DependencyProperty.RegisterReadOnly(nameof(Source), typeof(string), typeof(DownloadWindow), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty SourceProperty = SourceKey.DependencyProperty;
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            private set => SetValue(SourceKey, value);
        }

        DownloadWindow() : base()
        {
            this.RegisterHandler(null, i => i.GetChild<DownloadControl>(DownloadControlKey).Downloaded -= OnDownloaded);
            InitializeComponent();

            this.GetChild<DownloadControl>(DownloadControlKey).Downloaded += OnDownloaded;
        }

        public DownloadWindow(string title, string message, string source, string destination) : this()
        {
            Source 
                = source;
            Destination
                = destination;

            SetCurrentValue(MessageProperty,
                message);
            SetCurrentValue(TitleProperty,
                title);
        }

        void OnDownloaded(object sender, EventArgs<Result> e)
        {
            if (AutoClose)
                Close();
        }
    }
}