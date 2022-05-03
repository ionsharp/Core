using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    public class HistogramControl : ContentControl
    {
        readonly CancelTask<WriteableBitmap> refreshTask;

        static readonly DependencyPropertyKey HistogramKey = DependencyProperty.RegisterReadOnly(nameof(Histogram), typeof(Histogram), typeof(HistogramControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty HistogramProperty = HistogramKey.DependencyProperty;
        public Histogram Histogram
        {
            get => (Histogram)GetValue(HistogramProperty);
            private set => SetValue(HistogramKey, value);
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(WriteableBitmap), typeof(HistogramControl), new FrameworkPropertyMetadata(null, OnImageChanged));
        public WriteableBitmap Image
        {
            get => (WriteableBitmap)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        static void OnImageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<HistogramControl>().OnImageChanged(e);

        public HistogramControl() : base()
        {
            refreshTask = new(null, Refresh, true);
            Histogram = new();
        }

        async Task Refresh(WriteableBitmap input, CancellationToken token)
        {
            if (input != null)
            {
                var colors = new ColorMatrix(input);
                await Histogram.Read(colors);
            }
            else
            {
                Histogram.RedPoints
                    = null;
                Histogram.GreenPoints
                    = null;
                Histogram.BluePoints
                    = null;
                Histogram.SaturationPoints
                    = null;
                Histogram.LuminancePoints
                    = null;
            }
        }

        protected virtual void OnImageChanged(Value<WriteableBitmap> input) => _ = refreshTask.StartAsync(input.New);

        ICommand refreshCommand;
        public ICommand RefreshCommand
            => refreshCommand ??= new RelayCommand(() => _ = refreshTask.StartAsync(Image), () => Image != null);
    }
}