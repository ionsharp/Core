using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    public class BrushView : Control
    {
        public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(nameof(Brush), typeof(Brush), typeof(BrushView), new FrameworkPropertyMetadata(null, OnRender));
        public Brush Brush
        {
            get => (Brush)GetValue(BrushProperty);
            set => SetValue(BrushProperty, value);
        }

        static readonly DependencyPropertyKey PreviewKey = DependencyProperty.RegisterReadOnly(nameof(Preview), typeof(WriteableBitmap), typeof(BrushView), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty PreviewProperty = PreviewKey.DependencyProperty;
        public WriteableBitmap Preview
        {
            get => (WriteableBitmap)GetValue(PreviewProperty);
            private set => SetValue(PreviewKey, value);
        }

        public static readonly DependencyProperty PreviewColorProperty = DependencyProperty.Register(nameof(PreviewColor), typeof(Color), typeof(BrushView), new FrameworkPropertyMetadata(Colors.Black, OnRender));
        public Color PreviewColor
        {
            get => (Color)GetValue(PreviewColorProperty);
            set => SetValue(PreviewColorProperty, value);
        }

        public static readonly DependencyProperty PreviewHeightProperty = DependencyProperty.Register(nameof(PreviewHeight), typeof(int), typeof(BrushView), new FrameworkPropertyMetadata(16, OnRender));
        public int PreviewHeight
        {
            get => (int)GetValue(PreviewHeightProperty);
            set => SetValue(PreviewHeightProperty, value);
        }

        public static readonly DependencyProperty PreviewLengthProperty = DependencyProperty.Register(nameof(PreviewLength), typeof(int), typeof(BrushView), new FrameworkPropertyMetadata(1, OnRender));
        public int PreviewLength
        {
            get => (int)GetValue(PreviewLengthProperty);
            set => SetValue(PreviewLengthProperty, value);
        }
        
        public static readonly DependencyProperty PreviewSimulateProperty = DependencyProperty.Register(nameof(PreviewSimulate), typeof(bool), typeof(BrushView), new FrameworkPropertyMetadata(false, OnRender));
        public bool PreviewSimulate
        {
            get => (bool)GetValue(PreviewSimulateProperty);
            set => SetValue(PreviewSimulateProperty, value);
        }

        static readonly DependencyPropertyKey PreviewSizeKey = DependencyProperty.RegisterReadOnly(nameof(PreviewSize), typeof(DoubleSize), typeof(BrushView), new FrameworkPropertyMetadata(null, OnRender));
        public static readonly DependencyProperty PreviewSizeProperty = PreviewSizeKey.DependencyProperty;
        public DoubleSize PreviewSize
        {
            get => (DoubleSize)GetValue(PreviewSizeProperty);
            private set => SetValue(PreviewSizeKey, value);
        }

        public static readonly DependencyProperty PreviewWidthProperty = DependencyProperty.Register(nameof(PreviewWidth), typeof(int), typeof(BrushView), new FrameworkPropertyMetadata(16, OnRender));
        public int PreviewWidth
        {
            get => (int)GetValue(PreviewWidthProperty);
            set => SetValue(PreviewWidthProperty, value);
        }

        static void OnRender(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<BrushView>().Render();

        public BrushView() : base() 
        {
            this.RegisterHandler(i => Brush.If(j => { j.PropertyChanged -= OnBrushChanged; j.PropertyChanged += OnBrushChanged; }), i => Brush.If(j => j.PropertyChanged -= OnBrushChanged));

            Preview = BitmapFactory.New(PreviewWidth, PreviewHeight);
            PreviewSize = new(16, 16);
        }

        void Render()
        {
            if (Preview.PixelHeight != PreviewHeight || Preview.PixelWidth != PreviewWidth)
                Preview = BitmapFactory.New(PreviewWidth, PreviewHeight);

            else Preview.Clear(Colors.Transparent);

            if (Brush != null)
            {
                var result = Brush.GetBytes(PreviewLength);

                var result0 = new ColorMatrix(result.Transform(i => Color.FromArgb(i, 0, 0, 0)));
                var result1 = BitmapFactory.New(result0);
                //XScale/YScale
                var result2 = result1.Resize((PreviewLength.Double() * Brush.XScale).Int32(), (PreviewLength.Double() * Brush.YScale).Int32(), Interpolations.Bilinear);
                //Angle
                var result3 = result2.RotateFree(Brush.Angle);
                //Noise
                result3.ForEach((_, _, color) =>
                {
                    if (color.A > 0)
                    {
                        var amount = (Brush.Noise * 255).Coerce(255).Int32();

                        int oa = color.A;
                        int ia = Common.Random.Current.Next(-amount, amount + 1);

                        byte na = (oa - ia).Coerce(255).Byte();
                        return Color.FromArgb(na, color.R, color.G, color.B);
                    }
                    else return color;
                });

                var result4 = Brush.GetBytes(result3);

                if (!PreviewSimulate)
                {
                    Preview.Blend(result, new(0, 0), PreviewColor);
                }
                else
                {
                    var y1 = PreviewLength.Double() / 2.0;
                    var y2 = Preview.PixelHeight - 1 - (PreviewLength.Double() / 2.0);

                    var ease = Easing.Get(Ease.EaseInOutSine);

                    bool up = true;

                    var j = 0.0;
                    for (var x = PreviewLength; x < Preview.PixelWidth - PreviewLength; x++, j++)
                    {
                        var y01 = up ? y2 : y1;
                        var y02 = up ? y1 : y2;

                        var y = ease(y01, y02, j / Preview.PixelHeight.Double());
                        Preview.Blend(result, new Vector2<int>(x, y.Int32()), PreviewColor);

                        if (y >= y2)
                        {
                            j = 0;
                            up = true;
                        }
                        else if (y <= y1)
                        {
                            j = 0;
                            up = false;
                        }
                    }
                }
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == BrushProperty)
            {
                if (e.OldValue is Brush aBrush)
                {
                    aBrush.PropertyChanged -= OnBrushChanged;
                }

                if (e.NewValue is Brush bBrush)
                {
                    bBrush.PropertyChanged -= OnBrushChanged;
                    bBrush.PropertyChanged += OnBrushChanged;
                }
            }

            if (e.Property == PreviewHeightProperty || e.Property == PreviewWidthProperty)
                PreviewSize = new(PreviewHeight, PreviewWidth);
        }

        private void OnBrushChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Render();
        }
    }
}