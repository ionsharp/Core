using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Threading;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public delegate void DownloadEventHandler(object sender, EventArgs<Result> e);

    public class DownloadControl : Control
    {
        public static readonly ResourceKey<ProgressBar> ProgressBarStyleKey = new();

        public static readonly ResourceKey<TextBlock> TextBlockStyleKey = new();

        class Data
        {
            public readonly string Source;

            public readonly string Destination;

            public Data(string source, string destination)
            {
                Source
                    = source;
                Destination
                    = destination;
            }
        }

        readonly CancelTask<Data> downloadTask;

        readonly Stopwatch stopwatch = new();

        //...

        public event DownloadEventHandler Downloaded;

        //...

        public static readonly DependencyProperty AutoStartProperty = DependencyProperty.Register(nameof(AutoStart), typeof(bool), typeof(DownloadControl), new FrameworkPropertyMetadata(false, OnAutoStartChanged));
        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
        }
        static void OnAutoStartChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<DownloadControl>().OnAutoStartChanged(new(e));

        public static readonly DependencyProperty DestinationProperty = DependencyProperty.Register(nameof(Destination), typeof(string), typeof(DownloadControl), new FrameworkPropertyMetadata(string.Empty, OnDestinationChanged));
        public string Destination
        {
            get => (string)GetValue(DestinationProperty);
            set => SetValue(DestinationProperty, value);
        }
        static void OnDestinationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<DownloadControl>().OnDestinationChanged(new(e));

        static readonly DependencyPropertyKey MessageKey = DependencyProperty.RegisterReadOnly(nameof(Message), typeof(object), typeof(DownloadControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty MessageProperty = MessageKey.DependencyProperty;
        public object Message
        {
            get => (object)GetValue(MessageProperty);
            private set => SetValue(MessageKey, value);
        }

        public static readonly DependencyProperty MessageTemplateProperty = DependencyProperty.Register(nameof(MessageTemplate), typeof(DataTemplate), typeof(DownloadControl), new FrameworkPropertyMetadata(null));
        public DataTemplate MessageTemplate
        {
            get => (DataTemplate)GetValue(MessageTemplateProperty);
            set => SetValue(MessageTemplateProperty, value);
        }

        public static readonly DependencyProperty MessageTemplateSelectorProperty = DependencyProperty.Register(nameof(MessageTemplateSelector), typeof(DataTemplateSelector), typeof(DownloadControl), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector MessageTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(MessageTemplateSelectorProperty);
            set => SetValue(MessageTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(string), typeof(DownloadControl), new FrameworkPropertyMetadata(string.Empty, OnSourceChanged));
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<DownloadControl>().OnSourceChanged(new(e));

        static readonly DependencyPropertyKey ProgressKey = DependencyProperty.RegisterReadOnly(nameof(Progress), typeof(double), typeof(DownloadControl), new FrameworkPropertyMetadata(0.0));
        public static readonly DependencyProperty ProgressProperty = ProgressKey.DependencyProperty;
        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            private set => SetValue(ProgressKey, value);
        }

        static readonly DependencyPropertyKey SpeedKey = DependencyProperty.RegisterReadOnly(nameof(Speed), typeof(double), typeof(DownloadControl), new FrameworkPropertyMetadata(0.0));
        public static readonly DependencyProperty SpeedProperty = SpeedKey.DependencyProperty;
        public double Speed
        {
            get => (double)GetValue(SpeedProperty);
            private set => SetValue(SpeedKey, value);
        }

        static readonly DependencyPropertyKey ProcessedKey = DependencyProperty.RegisterReadOnly(nameof(Processed), typeof(string), typeof(DownloadControl), new FrameworkPropertyMetadata("0 MB / 0 MB"));
        public static readonly DependencyProperty ProcessedProperty = ProcessedKey.DependencyProperty;
        public string Processed
        {
            get => (string)GetValue(ProcessedProperty);
            private set => SetValue(ProcessedKey, value);
        }

        static readonly DependencyPropertyKey RemainingKey = DependencyProperty.RegisterReadOnly(nameof(Remaining), typeof(long), typeof(DownloadControl), new FrameworkPropertyMetadata(0L));
        public static readonly DependencyProperty RemainingProperty = RemainingKey.DependencyProperty;
        public long Remaining
        {
            get => (long)GetValue(RemainingProperty);
            private set => SetValue(RemainingKey, value);
        }

        //...

        public DownloadControl() : base()
        {
            downloadTask = new(null, DownloadAsync, true);
        }

        //...

        async Task DownloadAsync(Data data, CancellationToken token)
        {
            Result result = null;

            var watch = stopwatch;
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged
                    += new DownloadProgressChangedEventHandler(OnDownloadProgressChanged);

                Uri uri = data.Source.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(data.Source) : new Uri("http://" + data.Source);
                await Task.Run(new Action(() => Try.Invoke(() => System.IO.Directory.CreateDirectory(data.Destination), e => result = new Error($"Download failed: {e.Message}"))));

                if (result is not Error)
                {
                    await Try.InvokeAsync(async () =>
                    {
                        watch.Start();
                        await client.DownloadFileTaskAsync(uri, string.Concat(data.Destination, @"\", System.IO.Path.GetFileName(data.Source)));
                        watch.Reset();

                        result = new Success($"Download succeeded: '{data.Source}'");
                    },
                    e => result = new Error($"Download failed: {e.Message}"));
                }
            }
            Log.Write<DownloadControl>(result);
            OnDownloaded(result);
        }

        //...

        void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Speed
                = Math.Round(e.BytesReceived / 1024d / stopwatch.Elapsed.TotalSeconds, 3);
            Progress
                = Convert.ToDouble(e.ProgressPercentage);
            Processed
                = $"{(e.BytesReceived / 1024d / 1024d).ToString("0.00")} MB / {(e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00")} MB";
            Remaining
                = stopwatch.Elapsed.Left(e.TotalBytesToReceive, e.BytesReceived).TotalSeconds.Int64();
        }

        //...

        protected virtual void OnAutoStartChanged(Value<bool> input)
        {
            if (input.New)
            {
                if (!downloadTask.Started)
                    Start();
            }
        }

        protected virtual void OnDestinationChanged(Value<string> input)
        {
            if (AutoStart) Start();
        }

        protected virtual void OnDownloaded(Result input) => Downloaded?.Invoke(this, new(input));

        protected virtual void OnSourceChanged(Value<string> input)
        {
            if (AutoStart) Start();
        }

        //...

        public void Start() => _ = downloadTask.StartAsync(new(Source, Destination));
    }
}