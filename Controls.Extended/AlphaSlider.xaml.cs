using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AlphaSlider : UserControl
    {
        #region Properties

        WriteableBitmap TransparencyBitmap = new WriteableBitmap(24, 256, 96, 96, PixelFormats.Bgra32, null);

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<byte>> AlphaChanged;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(AlphaSlider), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
        /// <summary>
        /// 
        /// </summary>
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
            d.As<AlphaSlider>().OnColorChanged((Color)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(AlphaSlider), new PropertyMetadata((byte)255, new PropertyChangedCallback(OnAlphaChanged)));
        /// <summary>
        /// 
        /// </summary>
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
            d.As<AlphaSlider>().OnAlphaChanged((byte)e.NewValue);
        }

        #endregion

        #region AlphaSlider

        /// <summary>
        /// 
        /// </summary>
        public AlphaSlider()
        {
            InitializeComponent();
            PART_Image.Source = TransparencyBitmap;
        }

        #endregion

        #region Methods

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var alphaPercent = 100 * (PART_Image.ActualHeight - (e.GetPosition((IInputElement)sender)).Y) / PART_Image.ActualHeight;
            PART_Slider.Value = alphaPercent;
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ((IInputElement)sender).CaptureMouse();
                var alphaPercent = 100 * (PART_Image.ActualHeight - (e.GetPosition((IInputElement)sender)).Y) / PART_Image.ActualHeight;
                PART_Slider.Value = alphaPercent;
            }
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ((IInputElement)sender).ReleaseMouseCapture();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnAlphaChanged(byte Value)
        {
            PART_Slider.Value = Value / 2.55;
            AlphaChanged?.Invoke(this, new EventArgs<byte>(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnColorChanged(Color Value)
        {
            unsafe
            {
                TransparencyBitmap.Lock();
                var current = -1;
                var start = (byte*)(void*)TransparencyBitmap.BackBuffer;
                for (var r = 0; r < TransparencyBitmap.PixelHeight; r++)
                {
                    for (var c = 0; c < TransparencyBitmap.PixelWidth; c++)
                    {
                        current++;
                        *(start + current * 4 + 0) = Value.B;
                        *(start + current * 4 + 1) = Value.G;
                        *(start + current * 4 + 2) = Value.R;
                        *(start + current * 4 + 3) = (byte)(255 - r);
                    }
                }
                TransparencyBitmap.AddDirtyRect(new Int32Rect(0, 0, TransparencyBitmap.PixelWidth, TransparencyBitmap.PixelHeight));
                TransparencyBitmap.Unlock();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Alpha = Convert.ToByte(e.NewValue * 2.55);
        }

        #endregion
    }
}
