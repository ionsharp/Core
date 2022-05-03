using ImageMagick;
using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Imagin.Common.Controls
{
    public class SlideControl : Control
    {
        public enum Types
        {
            None,
            File,
            Folder
        }

        #region Properties

        readonly DispatcherTimer timer;

        readonly Storage.ItemCollection items = new(string.Empty, new Filter(ItemType.File));

        //...

        public static readonly DependencyProperty DefaultBackgroundProperty = DependencyProperty.Register(nameof(DefaultBackground), typeof(Brush), typeof(SlideControl), new FrameworkPropertyMetadata(SystemColors.ControlBrush));
        public Brush DefaultBackground
        {
            get => (Brush)GetValue(DefaultBackgroundProperty);
            set => SetValue(DefaultBackgroundProperty, value);
        }

        public static readonly DependencyProperty BackgroundBlurProperty = DependencyProperty.Register(nameof(BackgroundBlur), typeof(bool), typeof(SlideControl), new FrameworkPropertyMetadata(true));
        public bool BackgroundBlur
        {
            get => (bool)GetValue(BackgroundBlurProperty);
            set => SetValue(BackgroundBlurProperty, value);
        }

        public static readonly DependencyProperty BackgroundBlurRadiusProperty = DependencyProperty.Register(nameof(BackgroundBlurRadius), typeof(double), typeof(SlideControl), new FrameworkPropertyMetadata(100.0));
        public double BackgroundBlurRadius
        {
            get => (double)GetValue(BackgroundBlurRadiusProperty);
            set => SetValue(BackgroundBlurRadiusProperty, value);
        }

        public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(nameof(BackgroundOpacity), typeof(double), typeof(SlideControl), new FrameworkPropertyMetadata(1.0));
        public double BackgroundOpacity
        {
            get => (double)GetValue(BackgroundOpacityProperty);
            set => SetValue(BackgroundOpacityProperty, value);
        }

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof(Interval), typeof(TimeSpan), typeof(SlideControl), new FrameworkPropertyMetadata(TimeSpan.FromSeconds(3), OnIntervalChanged));
        public TimeSpan Interval
        {
            get => (TimeSpan)GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }
        static void OnIntervalChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<SlideControl>().OnIntervalChanged(new Value<TimeSpan>(e));

        public static readonly DependencyProperty PauseOnMouseOverProperty = DependencyProperty.Register(nameof(PauseOnMouseOver), typeof(bool), typeof(SlideControl), new FrameworkPropertyMetadata(true));
        public bool PauseOnMouseOver
        {
            get => (bool)GetValue(PauseOnMouseOverProperty);
            set => SetValue(PauseOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(SlideControl), new FrameworkPropertyMetadata(string.Empty, OnPathChanged));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<SlideControl>().OnPathChanged(e);

        static readonly DependencyPropertyKey PathTypeKey = DependencyProperty.RegisterReadOnly(nameof(PathType), typeof(Types), typeof(SlideControl), new FrameworkPropertyMetadata(Types.None));
        public static readonly DependencyProperty PathTypeProperty = PathTypeKey.DependencyProperty;
        public Types PathType
        {
            get => (Types)GetValue(PathTypeProperty);
            private set => SetValue(PathTypeKey, value);
        }

        public static readonly DependencyProperty ScalingModeProperty = DependencyProperty.Register(nameof(ScalingMode), typeof(BitmapScalingMode), typeof(SlideControl), new FrameworkPropertyMetadata(BitmapScalingMode.HighQuality));
        public BitmapScalingMode ScalingMode
        {
            get => (BitmapScalingMode)GetValue(ScalingModeProperty);
            set => SetValue(ScalingModeProperty, value);
        }

        static readonly DependencyPropertyKey SelectedImageKey = DependencyProperty.RegisterReadOnly(nameof(SelectedImage), typeof(string), typeof(SlideControl), new FrameworkPropertyMetadata(default(string), OnSelectedImageChanged));
        public static readonly DependencyProperty SelectedImageProperty = SelectedImageKey.DependencyProperty;
        public string SelectedImage
        {
            get => (string)GetValue(SelectedImageProperty); 
            private set => SetValue(SelectedImageKey, value); 
        }
        static void OnSelectedImageChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<SlideControl>().OnSelectedImageChanged(e);

        static readonly DependencyPropertyKey SelectedImageSourceKey = DependencyProperty.RegisterReadOnly(nameof(SelectedImageSource), typeof(ImageSource), typeof(SlideControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedImageSourceProperty = SelectedImageSourceKey.DependencyProperty;
        public ImageSource SelectedImageSource
        {
            get => (ImageSource)GetValue(SelectedImageSourceProperty);
            private set => SetValue(SelectedImageSourceKey, value);
        }

        public static readonly DependencyProperty TransitionProperty = DependencyProperty.Register(nameof(Transition), typeof(Transitions), typeof(SlideControl), new FrameworkPropertyMetadata(Transitions.LeftReplace));
        public Transitions Transition
        {
            get => (Transitions)GetValue(TransitionProperty);
            set => SetValue(TransitionProperty, value);
        }

        #endregion

        #region SlideControl

        public SlideControl() : base()
        {
            this.RegisterHandler(OnLoaded, OnUnloaded);

            timer = new DispatcherTimer { Interval = Interval };
            timer.Tick += OnTick;
        }

        #endregion

        #region Methods

        void OnLoaded()
        {
            items.Subscribe();
            _ = items.RefreshAsync(Path);
        }

        void OnUnloaded()
        {
            items.Unsubscribe();
            items.Clear();
        }

        void OnTick(object sender, object e)
        {
            if (PathType != Types.Folder)
            {
                timer.Stop();
                return;
            }

            NextCommand.Execute(null);
        }

        //...

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (PauseOnMouseOver)
                timer.Stop();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (PauseOnMouseOver)
                timer.Start();
        }

        //...

        protected virtual void OnIntervalChanged(Value<TimeSpan> input)
        {
            timer.Interval = input.New.Coerce(TimeSpan.MaxValue, TimeSpan.FromSeconds(3));
        }

        protected virtual void OnPathChanged(Value<string> input)
        {
            timer.Stop();
            var type = Computer.GetType(input.New);
            switch (type)
            {
                case ItemType.File:
                    PathType = Types.File;
                    SelectedImage = input.New;
                    break;

                case ItemType.Folder:
                    PathType = Types.Folder;
                    _ = items.RefreshAsync(input.New);
                    timer.Start();
                    break;

                case ItemType.Shortcut:
                    if (Shortcut.TargetsFolder(input.New))
                    {
                        PathType = Types.Folder;
                        _ = items.RefreshAsync(Shortcut.TargetPath(input.New));
                        timer.Start();
                    }

                    goto default;

                default:
                    PathType = Types.None;
                    break;
            }
        }

        protected async virtual void OnSelectedImageChanged(Value<string> input)
            => SelectedImageSource = await Open(input.New);

        //...

        async Task<BitmapSource> Open(string filePath)
        {
            var result = await Task.Run(() =>
            {
                try
                {
                    return new MagickImage(filePath);
                }
                catch (Exception e)
                {
                    Log.Write<SlideControl>(e);
                    return null;
                }
            });
            return result?.ToBitmapSource();
        }

        //...

        void Next()
        {
            if (items.Count <= 1)
                return;

            var item = items.FirstOrDefault(i => i.Path == SelectedImage) ?? items[0];
            var index = items.IndexOf(item);

            index++;
            index = index > items.Count - 1 ? 0 : index;

            SelectedImage = items[index].Path;
        }

        void Previous()
        {
            if (items.Count <= 1)
                return;

            var item = items.FirstOrDefault(i => i.Path == SelectedImage) ?? items[0];
            var index = items.IndexOf(item);

            index--;
            index = index < 0 ? items.Count - 1 : index;

            SelectedImage = items[index].Path;
        }

        //...

        ICommand nextCommand;
        public ICommand NextCommand => nextCommand ??= new RelayCommand(Next, () => PathType == Types.Folder && items.Count > 1);

        ICommand previousCommand;
        public ICommand PreviousCommand => previousCommand ??= new RelayCommand(Previous, () => PathType == Types.Folder && items.Count > 1);

        #endregion
    }
}