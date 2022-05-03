using Imagin.Common.Linq;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Imagin.Common.Controls
{
    public partial class LoadWindow : Window
    {
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof(Delay), typeof(TimeSpan), typeof(LoadWindow), new FrameworkPropertyMetadata(TimeSpan.Zero));
        public TimeSpan Delay
        {
            get => (TimeSpan)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof(IsIndeterminate), typeof(bool), typeof(LoadWindow), new FrameworkPropertyMetadata(true));
        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(LoadWindow), new FrameworkPropertyMetadata(null));
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(LoadWindow), new FrameworkPropertyMetadata(0d));
        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        public LoadWindow() : base() => InitializeComponent();

        public LoadWindow(TimeSpan delay, string message, Action action) : this()
        {
            SetCurrentValue(DelayProperty, 
                delay);
            SetCurrentValue(MessageProperty, 
                message);
            _ = Start(action);
        }

        async Task Start(Action action)
        {
            if (Delay > TimeSpan.Zero)
                await Delay.TrySleep();

            action();
            //await Task.Run(action);
            Close();
        }
    }
}