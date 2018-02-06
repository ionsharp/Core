using Imagin.Common.Drawing;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ColorSelector : System.Windows.Controls.UserControl
    {
        #region Enums

        enum ColorChangeSourceType
        {
            ColorPropertySet,
            MouseDown,
            SliderMove,
        }

        #endregion

        #region Properties

        #region Dependency

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
        public static DependencyProperty IlluminantProperty = DependencyProperty.Register("Illuminant", typeof(Illuminant), typeof(ColorSelector), new FrameworkPropertyMetadata(Illuminant.Default, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIlluminantChanged));
        /// <summary>
        /// 
        /// </summary>
        public Illuminant Illuminant
        {
            get
            {
                return (Illuminant)GetValue(IlluminantProperty);
            }
            set
            {
                SetValue(IlluminantProperty, value);
            }
        }
        static void OnIlluminantChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorSelector>().OnIlluminantChanged((Illuminant)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedComponentProperty = DependencyProperty.Register("SelectedComponent", typeof(SelectableComponentModel), typeof(ColorSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedComponentChanged));
        /// <summary>
        /// 
        /// </summary>
        public SelectableComponentModel SelectedComponent
        {
            get
            {
                return (SelectableComponentModel)GetValue(SelectedComponentProperty);
            }
            set
            {
                SetValue(SelectedComponentProperty, value);
            }
        }
        static void OnSelectedComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorSelector>().OnSelectedComponentChanged((SelectableComponentModel)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ModelsProperty = DependencyProperty.Register("Models", typeof(ColorSpaceCollection), typeof(ColorSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ColorSpaceCollection Models
        {
            get
            {
                return (ColorSpaceCollection)GetValue(ModelsProperty);
            }
            set
            {
                SetValue(ModelsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ObserverProperty = DependencyProperty.Register("Observer", typeof(ObserverAngle), typeof(ColorSelector), new FrameworkPropertyMetadata(ObserverAngle.Two, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ObserverAngle Observer
        {
            get
            {
                return (ObserverAngle)GetValue(ObserverProperty);
            }
            set
            {
                SetValue(ObserverProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionRingProperty = DependencyProperty.Register("SelectionRing", typeof(ColorSelectorRing), typeof(ColorSelector), new PropertyMetadata(ColorSelectorRing.BlackAndWhite, OnSelectionRingChanged));
        /// <summary>
        /// 
        /// </summary>
        public ColorSelectorRing SelectionRing
        {
            get
            {
                return (ColorSelectorRing)GetValue(SelectionRingProperty);
            }
            set
            {
                SetValue(SelectionRingProperty, value);
            }
        }
        static void OnSelectionRingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorSelector>().OnSelectionRingChanged((ColorSelectorRing)e.NewValue);
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

        //SelectableComponentModel
        void OnSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ColorChangeSource = ColorChangeSourceType.SliderMove;
            if (ProcessSliderEvents)
            {
                ProcessSliderEvents = false;
                Color = SelectedComponent.ColorAtPoint(SelectionPoint, (int)e.NewValue).WithAlpha(PART_AlphaSlider.Alpha);
                UpdatePlane(SelectedComponent.GetValue(Color).Round().ToInt32());
                ProcessSliderEvents = true;
            }
            ColorChangeSource = ColorChangeSourceType.ColorPropertySet;
        }

        void OnSliderMouseDown(object sender, MouseButtonEventArgs e)
        {
            var y = (e.GetPosition((IInputElement)sender)).Y;

            var proportion = 1 - y / 255;
            var range = SelectedComponent.Maximum - SelectedComponent.Minimum;

            PART_Slider.Value = SelectedComponent.Minimum + proportion * range;
        }

        void OnSliderMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ((IInputElement)sender).CaptureMouse();

                var y = (e.GetPosition((IInputElement)sender)).Y;

                var proportion = 1 - y / 255;
                var range = SelectedComponent.Maximum - SelectedComponent.Minimum;

                PART_Slider.Value = SelectedComponent.Minimum + proportion * range;
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
            PART_SelectionEllipse.Stroke = new SolidColorBrush(new Hsb(Color).B > 60d ? Colors.Black : Colors.White);
        }

        void OnPlaneMouseDown(Point selectionPoint)
        {
            if (SelectedComponent != null)
            {
                SelectionPoint = selectionPoint;

                double w = PART_ColorPlane.ActualWidth / 2, h = PART_ColorPlane.ActualHeight / 2;

                SelectionTransform.X = (SelectionPoint.X - w).Coerce(w, -w);
                SelectionTransform.Y = (SelectionPoint.Y - h).Coerce(h, -h);

                SelectionPoint = new Point(SelectionPoint.X.Coerce(255d), SelectionPoint.Y.Coerce(255d));

                var NewColor = SelectedComponent.ColorAtPoint(SelectionPoint, (int)PART_Slider.Value);
                if (!SelectedComponent.IsUniform)
                    SelectedComponent.UpdateSlider(NormalPane, NewColor);

                Color = NewColor.WithAlpha(PART_AlphaSlider.Alpha);
            }
        }

        void UpdatePlane(int ComponentValue)
        {
            if (LastComponentValue != ComponentValue || LastComponentHashCode != SelectedComponent.GetHashCode())
            {
                SelectedComponent.UpdatePlane(SelectionPane, ComponentValue);
                LastComponentValue = ComponentValue;
                LastComponentHashCode = SelectedComponent.GetHashCode();
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// 
        /// </summary>
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
                if (SelectedComponent != null)
                {
                    if (ColorChangeSource == ColorChangeSourceType.ColorPropertySet)
                    {
                        UpdatePlane(SelectedComponent.GetValue(NewValue).Round().ToInt32());

                        SelectionPoint = SelectedComponent.PointFromColor(NewValue);
                        SelectionTransform.X = SelectionPoint.X - (SelectionPane.PixelWidth / 2.0);
                        SelectionTransform.Y = SelectionPoint.Y - (SelectionPane.PixelHeight / 2.0);

                        PART_Slider.Value = SelectedComponent.GetValue(NewValue);

                        if (!SelectedComponent.IsUniform)
                            SelectedComponent.UpdateSlider(NormalPane, NewValue);
                    }
                    if (SelectionRing == ColorSelectorRing.BlackOrWhite)
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
        protected virtual void OnIlluminantChanged(Illuminant Value)
        {
            UpdatePlane(SelectedComponent.GetValue(Color).Round().ToInt32());
            SelectedComponent.UpdateSlider(NormalPane, Color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSelectedComponentChanged(SelectableComponentModel Value)
        {
            SelectionPoint = Value.PointFromColor(Color);
            SelectionTransform.X = SelectionPoint.X - (PART_ColorPlane.ActualWidth / 2);
            SelectionTransform.Y = SelectionPoint.Y - (PART_ColorPlane.ActualHeight / 2);

            ProcessSliderEvents = false;

            PART_Slider.Minimum = Value.Minimum;
            PART_Slider.Maximum = Value.Maximum;
            PART_Slider.Value = Value.GetValue(Color);

            ProcessSliderEvents = true;

            Value.UpdatePlane(SelectionPane, Value.GetValue(Color).Round().ToInt32());
            Value.UpdateSlider(NormalPane, Color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSelectionRingChanged(ColorSelectorRing Value)
        {
            switch (Value)
            {
                case ColorSelectorRing.Black:
                    PART_SelectionEllipse.Stroke = new SolidColorBrush(Colors.Black);
                    PART_SelectionOuterEllipse.Visibility = Visibility.Collapsed;
                    break;
                case ColorSelectorRing.White:
                    PART_SelectionEllipse.Stroke = new SolidColorBrush(Colors.White);
                    PART_SelectionOuterEllipse.Visibility = Visibility.Collapsed;
                    break;
                case ColorSelectorRing.BlackAndWhite:
                    PART_SelectionEllipse.Stroke = new SolidColorBrush(Colors.White);
                    PART_SelectionOuterEllipse.Visibility = Visibility.Visible;
                    break;
                case ColorSelectorRing.BlackOrWhite:
                    AdjustRing(Color);
                    PART_SelectionOuterEllipse.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        #endregion

        #endregion
    }
}
