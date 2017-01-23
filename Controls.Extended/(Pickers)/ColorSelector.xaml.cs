using Imagin.Common.Extensions;
using Imagin.Common.Input;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Diagnostics;
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
    public partial class ColorSelector : UserControl
    {
        #region Enums

        enum ColorChangeSourceType
        {
            ColorPropertySet,
            MouseDown,
            SliderMove,
        }

        /// <summary>
        /// 
        /// </summary>
        public enum SelectionRingType
        {
            /// <summary>
            /// 
            /// </summary>
            White,
            /// <summary>
            /// 
            /// </summary>
            Black,
            /// <summary>
            /// 
            /// </summary>
            BlackAndWhite,
            /// <summary>
            /// 
            /// </summary>
            BlackOrWhite
        }

        #endregion

        #region Properties

        #region Private

        readonly TranslateTransform SelectionTransform = new TranslateTransform();

        readonly WriteableBitmap SelectionPane = new WriteableBitmap(256, 256, 96, 96, PixelFormats.Bgr24, null);

        readonly WriteableBitmap NormalPane = new WriteableBitmap(24, 256, 96, 96, PixelFormats.Bgr24, null);

        ColorChangeSourceType ColorChangeSource = ColorChangeSourceType.ColorPropertySet;

        bool ProcessSliderEvents
        {
            get; set;
        }

        int LastComponentValue = -1;

        int LastComponentHashCode = 0;

        Point SelectionPoint
        {
            get; set;
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<byte>> AlphaChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Color>> ColorChanged;

        #endregion

        #region Dependency

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorSelector), new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
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
            d.As<ColorSelector>().OnColorChanged((Color)e.OldValue, (Color)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NormalComponentProperty = DependencyProperty.Register("NormalComponent", typeof(NormalComponentModel), typeof(ColorSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnComponentChanged));
        /// <summary>
        /// 
        /// </summary>
        public NormalComponentModel NormalComponent
        {
            get
            {
                return (NormalComponentModel)GetValue(NormalComponentProperty);
            }
            set
            {
                SetValue(NormalComponentProperty, value);
            }
        }
        static void OnComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorSelector>().OnComponentChanged((NormalComponentModel)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionRingProperty = DependencyProperty.Register("SelectionRing", typeof(SelectionRingType), typeof(ColorSelector), new PropertyMetadata(SelectionRingType.BlackAndWhite, OnSelectionRingChanged));
        /// <summary>
        /// 
        /// </summary>
        public SelectionRingType SelectionRing
        {
            get
            {
                return (SelectionRingType)GetValue(SelectionRingProperty);
            }
            set
            {
                SetValue(SelectionRingProperty, value);
            }
        }
        static void OnSelectionRingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorSelector>().OnSelectionRingChanged((SelectionRingType)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(ColorSelector), new PropertyMetadata((byte)255));
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowSliderProperty = DependencyProperty.Register("ShowSlider", typeof(bool), typeof(ColorSelector), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowSlider
        {
            get
            {
                return (bool)GetValue(ShowSliderProperty);
            }
            set
            {
                SetValue(ShowSliderProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowAlphaProperty = DependencyProperty.Register("ShowAlpha", typeof(bool), typeof(ColorSelector), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowAlpha
        {
            get
            {
                return (bool)GetValue(ShowAlphaProperty);
            }
            set
            {
                SetValue(ShowAlphaProperty, value);
            }
        }

        #endregion

        #endregion

        #region ColorSelector

        /// <summary>
        /// 
        /// </summary>
        public ColorSelector()
        {
            InitializeComponent();

            PART_ColorPlane.Source = SelectionPane;
            PART_Image.Source = NormalPane;

            PART_SelectionEllipse.RenderTransform = SelectionTransform;
            PART_SelectionOuterEllipse.RenderTransform = SelectionTransform;

            ProcessSliderEvents = true;
        }

        #endregion

        #region Methods

        #region Handlers

        void OnPlaneMouseDown(object sender, MouseButtonEventArgs e)
        {
            ColorChangeSource = ColorChangeSourceType.MouseDown;
            OnPlaneMouseDown(e.GetPosition((IInputElement)sender));
            ColorChangeSource = ColorChangeSourceType.ColorPropertySet;
        }

        void OnPlaneMouseMove(object sender, MouseEventArgs e)
        {
            ColorChangeSource = ColorChangeSourceType.MouseDown;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ((IInputElement)sender).CaptureMouse();
                var Position = e.GetPosition((IInputElement)sender);
                OnPlaneMouseDown(Position);
            }
            ColorChangeSource = ColorChangeSourceType.ColorPropertySet;
        }

        void OnPlaneMouseUp(object sender, MouseEventArgs e)
        {
            ((IInputElement)sender).ReleaseMouseCapture();
        }

        void OnSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ColorChangeSource = ColorChangeSourceType.SliderMove;
            if (ProcessSliderEvents)
            {
                ProcessSliderEvents = false;
                Color = NormalComponent.ColorAtPoint(SelectionPoint, (int)e.NewValue).WithAlpha(PART_AlphaSlider.Alpha);
                UpdatePlane(NormalComponent.GetValue(Color));
                ProcessSliderEvents = true;
            }
            ColorChangeSource = ColorChangeSourceType.ColorPropertySet;
        }

        void OnSliderMouseDown(object sender, MouseButtonEventArgs e)
        {
            var y = (e.GetPosition((IInputElement)sender)).Y;

            var proportion = 1 - y / 255;
            var range = NormalComponent.MaxValue - NormalComponent.MinValue;

            PART_Slider.Value = NormalComponent.MinValue + proportion * range;
        }

        void OnSliderMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ((IInputElement)sender).CaptureMouse();

                var y = (e.GetPosition((IInputElement)sender)).Y;

                var proportion = 1 - y / 255;
                var range = NormalComponent.MaxValue - NormalComponent.MinValue;

                PART_Slider.Value = NormalComponent.MinValue + proportion * range;
            }
        }

        void OnSliderMouseUp(object sender, MouseEventArgs e)
        {
            ((IInputElement)sender).ReleaseMouseCapture();
        }

        #endregion

        #region Private

        void AdjustRing(Color Color)
        {
            PART_SelectionEllipse.Stroke = new SolidColorBrush(Hsb.FromColor(Color).B > .6 ? Colors.Black : Colors.White);
        }

        void OnPlaneMouseDown(Point selectionPoint)
        {
            SelectionPoint = selectionPoint;

            double w = PART_ColorPlane.ActualWidth / 2, h = PART_ColorPlane.ActualHeight / 2;

            SelectionTransform.X = (SelectionPoint.X - w).Coerce(w, -w);
            SelectionTransform.Y = (SelectionPoint.Y - h).Coerce(h, -h);

            SelectionPoint = new Point(SelectionPoint.X.Coerce(255d), SelectionPoint.Y.Coerce(255d));

            var NewColor = NormalComponent.ColorAtPoint(SelectionPoint, (int)PART_Slider.Value);
            if (!NormalComponent.IsNormalIndependantOfColor)
                NormalComponent.UpdateSlider(NormalPane, NewColor);

            Color = NewColor.WithAlpha(PART_AlphaSlider.Alpha);
        }

        void UpdatePlane(int ComponentValue)
        {
            if (LastComponentValue != ComponentValue || LastComponentHashCode != NormalComponent.GetHashCode())
            {
                NormalComponent.UpdatePlane(SelectionPane, ComponentValue);
                LastComponentValue = ComponentValue;
                LastComponentHashCode = NormalComponent.GetHashCode();
            }
        }

        #endregion

        #region Public

        public void IncrementSlider()
        {
            PART_Slider.Value++;
        }

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAlphaChanged(object sender, EventArgs<byte> e)
        {
            SetValue(ColorProperty, Color.WithAlpha(e.Value));
            ColorChanged?.Invoke(this, new EventArgs<Color>(Color));
            Alpha = e.Value;
            AlphaChanged?.Invoke(this, new EventArgs<byte>(Alpha));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnColorChanged(Color OldValue, Color NewValue)
        {
            if (OldValue != NewValue)
            {
                if (NormalComponent != null)
                {
                    if (ColorChangeSource == ColorChangeSourceType.ColorPropertySet)
                    {
                        UpdatePlane(NormalComponent.GetValue(NewValue));
                        SelectionPoint = NormalComponent.PointFromColor(NewValue);
                        SelectionTransform.X = SelectionPoint.X - (SelectionPane.PixelWidth / 2.0);
                        SelectionTransform.Y = SelectionPoint.Y - (SelectionPane.PixelHeight / 2.0);

                        PART_Slider.Value = NormalComponent.GetValue(NewValue);

                        if (!NormalComponent.IsNormalIndependantOfColor)
                            NormalComponent.UpdateSlider(NormalPane, NewValue);
                    }
                    if (SelectionRing == SelectionRingType.BlackOrWhite)
                        AdjustRing(NewValue);
                }
                PART_AlphaSlider.Alpha = NewValue.A;

                ColorChanged?.Invoke(this, new EventArgs<Color>(NewValue));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnComponentChanged(NormalComponentModel Value)
        {
            SelectionPoint = Value.PointFromColor(Color);
            SelectionTransform.X = SelectionPoint.X - (PART_ColorPlane.ActualWidth / 2);
            SelectionTransform.Y = SelectionPoint.Y - (PART_ColorPlane.ActualHeight / 2);

            ProcessSliderEvents = false;

            PART_Slider.Minimum = Value.MinValue;
            PART_Slider.Maximum = Value.MaxValue;
            PART_Slider.Value = Value.GetValue(Color);

            ProcessSliderEvents = true;

            Value.UpdateSlider(NormalPane, Color);
            Value.UpdatePlane(SelectionPane, Value.GetValue(Color));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSelectionRingChanged(SelectionRingType Value)
        {
            switch (Value)
            {
                case SelectionRingType.Black:
                    PART_SelectionEllipse.Stroke = new SolidColorBrush(Colors.Black);
                    PART_SelectionOuterEllipse.Visibility = Visibility.Collapsed;
                    break;
                case SelectionRingType.White:
                    PART_SelectionEllipse.Stroke = new SolidColorBrush(Colors.White);
                    PART_SelectionOuterEllipse.Visibility = Visibility.Collapsed;
                    break;
                case SelectionRingType.BlackAndWhite:
                    PART_SelectionEllipse.Stroke = new SolidColorBrush(Colors.White);
                    PART_SelectionOuterEllipse.Visibility = Visibility.Visible;
                    break;
                case SelectionRingType.BlackOrWhite:
                    AdjustRing(Color);
                    PART_SelectionOuterEllipse.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        #endregion

        #endregion
    }
}
