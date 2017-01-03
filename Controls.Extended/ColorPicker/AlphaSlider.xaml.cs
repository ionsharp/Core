using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    public partial class AlphaSlider : UserControl
    {
        #region Properties

        public event EventHandler<EventArgs<byte>> AlphaChanged;

        WriteableBitmap TransparencyBitmap = new WriteableBitmap(24, 256, 96, 96, PixelFormats.Bgra32, null);

        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(AlphaSlider), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
        public Color Color
        {
            get
            {
                return (Color)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }
        static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var AlphaSlider = (AlphaSlider)d;
            AlphaSlider.OnColorChanged((Color)e.NewValue);
        }

        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(AlphaSlider), new PropertyMetadata((byte)255, new PropertyChangedCallback(OnAlphaChanged)));
        public byte Alpha
        {
            get
            {
                return (byte)GetValue(AlphaProperty);
            }
            set
            {
                SetValue(AlphaProperty, value);
            }
        }
        static void OnAlphaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AlphaSlider AlphaSlider = (AlphaSlider)d;
            AlphaSlider.PART_Slider.Value = (byte)e.NewValue / 2.55;
            if (AlphaSlider.AlphaChanged != null)
                AlphaSlider.AlphaChanged(AlphaSlider, new EventArgs<byte>(AlphaSlider.Alpha));
        }

        #endregion

        #region AlphaSlider

        public AlphaSlider()
        {
            InitializeComponent();
            this.PART_Image.Source = TransparencyBitmap;
        }

        #endregion

        #region Methods

        void OnColorChanged(Color color)
        {
            unsafe
            {
                TransparencyBitmap.Lock();
                int currentPixel = -1;
                byte* pStart = (byte*)(void*)TransparencyBitmap.BackBuffer;
                for (int iRow = 0; iRow < TransparencyBitmap.PixelHeight; iRow++)
                {
                    for (int iCol = 0; iCol < TransparencyBitmap.PixelWidth; iCol++)
                    {
                        currentPixel++;
                        *(pStart + currentPixel * 4 + 0) = color.B;
                        *(pStart + currentPixel * 4 + 1) = color.G;
                        *(pStart + currentPixel * 4 + 2) = color.R;
                        *(pStart + currentPixel * 4 + 3) = (byte)(255 - iRow);
                    }
                }
                TransparencyBitmap.AddDirtyRect(new Int32Rect(0, 0, TransparencyBitmap.PixelWidth, TransparencyBitmap.PixelHeight));
                TransparencyBitmap.Unlock();
            }
        }

        void OnImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            var alphaPercent = 100 * (PART_Image.ActualHeight - (e.GetPosition((IInputElement)sender)).Y) / PART_Image.ActualHeight;
            PART_Slider.Value = alphaPercent;
        }

        void OnImageMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ((IInputElement)sender).CaptureMouse();
                var alphaPercent = 100 * (PART_Image.ActualHeight - (e.GetPosition((IInputElement)sender)).Y) / PART_Image.ActualHeight;
                PART_Slider.Value = alphaPercent;
            }
        }

        void OnImageMouseUp(object sender, MouseButtonEventArgs e)
        {
            ((IInputElement)sender).ReleaseMouseCapture();
        }

        void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Alpha = Convert.ToByte(e.NewValue * 2.55);
        }

        #endregion
    }
}
