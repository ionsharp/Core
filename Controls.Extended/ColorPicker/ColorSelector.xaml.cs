using Imagin.Common.Events;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    public partial class ColorSelector : UserControl
    {
        #region Properties

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

        public event EventHandler<EventArgs<Color>> ColorChanged;

        #region Dependency

        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorSelector), new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
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

            var c = (Color)e.NewValue;
            var oldC = (Color)e.OldValue;
            if (c != oldC)
            {
                var cs = (ColorSelector)d;
                cs.OnColorChanged(c);
                cs.PART_AlphaSlider.Alpha = c.A;
            }
        }

        public static DependencyProperty NormalComponentProperty = DependencyProperty.Register("NormalComponent", typeof(NormalComponentModel), typeof(ColorSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnNormalComponentChanged));
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
        static void OnNormalComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var cc = (NormalComponentModel)e.NewValue;
                var cs = (ColorSelector)d;
                cs.OnNormalComponentChanged(cc);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static DependencyProperty SelectionRingProperty = DependencyProperty.Register("SelectionRing", typeof(SelectionRingType), typeof(ColorSelector), new PropertyMetadata(SelectionRingType.BlackAndWhite, OnSelectionRingChanged));
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
            var colorSelector = (ColorSelector)d;
            var SelectionRing = (SelectionRingType)e.NewValue;
            colorSelector.OnSelectionRingModeChanged(SelectionRing);
        }

        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(ColorSelector), new PropertyMetadata((byte)255));
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

        public static DependencyProperty ShowSliderProperty = DependencyProperty.Register("ShowSlider", typeof(bool), typeof(ColorSelector), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ShowAlphaSliderProperty = DependencyProperty.Register("ShowAlphaSlider", typeof(bool), typeof(ColorSelector), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowAlphaSlider
        {
            get
            {
                return (bool)GetValue(ShowAlphaSliderProperty);
            }
            set
            {
                SetValue(ShowAlphaSliderProperty, value);
            }
        }

        #endregion

        #endregion

        #region Types

        enum ColorChangeSourceType
        {
            ColorPropertySet,
            MouseDown,
            SliderMove,
        }

        public enum SelectionRingType
        {
            White,
            Black,
            BlackAndWhite,
            BlackOrWhite
        }

        #endregion

        #region ColorSelector

        public ColorSelector()
        {
            InitializeComponent();

            this.PART_ColorPlane.MouseDown += this.OnColorPlaneMouseDown;
            this.PART_ColorPlane.Source = SelectionPane;

            this.PART_Image.Source = NormalPane;

            this.PART_SelectionEllipse.RenderTransform = SelectionTransform;
            this.PART_SelectionOuterEllipse.RenderTransform = SelectionTransform;

            this.ProcessSliderEvents = true;
        }

        #endregion

        #region Methods

        void UpdateColorPlaneBitmap(int ComponentValue)
        {
            if (LastComponentValue != ComponentValue || LastComponentHashCode != NormalComponent.GetHashCode())
            {
                NormalComponent.UpdatePlane(SelectionPane, ComponentValue);
                LastComponentValue = ComponentValue;
                LastComponentHashCode = NormalComponent.GetHashCode();
            }
        }

        void OnColorChanged(Color color)
        {
            if (NormalComponent == null)
                return;

            if (ColorChangeSource == ColorChangeSourceType.ColorPropertySet)
            {
                UpdateColorPlaneBitmap(NormalComponent.GetValue(color));
                SelectionPoint = NormalComponent.PointFromColor(color);
                SelectionTransform.X = SelectionPoint.X - (SelectionPane.PixelWidth / 2.0);
                SelectionTransform.Y = SelectionPoint.Y - (SelectionPane.PixelHeight / 2.0);

                PART_Slider.Value = NormalComponent.GetValue(color);

                if (!NormalComponent.IsNormalIndependantOfColor)
                    NormalComponent.UpdateSlider(NormalPane, color);
            }
            if (this.SelectionRing == SelectionRingType.BlackOrWhite)
                AdjustSelectionRing(color);

            if (ColorChanged != null)
                ColorChanged(this, new EventArgs<Color>(color));
        }

        void AdjustSelectionRing(Color c)
        {
            if (Hsb.FromColor(Color).B > .6)
                PART_SelectionEllipse.Stroke = new SolidColorBrush(Colors.Black);
            else PART_SelectionEllipse.Stroke = new SolidColorBrush(Colors.White);
        }

        void OnNormalComponentChanged(NormalComponentModel NormalComponentModel)
        {
            SelectionPoint = NormalComponentModel.PointFromColor(Color);
            SelectionTransform.X = SelectionPoint.X - (PART_ColorPlane.ActualWidth / 2);
            SelectionTransform.Y = SelectionPoint.Y - (PART_ColorPlane.ActualHeight / 2);

            ProcessSliderEvents = false;

            PART_Slider.Minimum = NormalComponentModel.MinValue;
            PART_Slider.Maximum = NormalComponentModel.MaxValue;
            PART_Slider.Value = NormalComponentModel.GetValue(Color);

            ProcessSliderEvents = true;

            NormalComponentModel.UpdateSlider(NormalPane, Color);
            NormalComponentModel.UpdatePlane(SelectionPane, NormalComponentModel.GetValue(Color));
        }

        void OnSelectionRingModeChanged(SelectionRingType selectionRingMode)
        {
            switch (selectionRingMode)
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
                    AdjustSelectionRing(Color);

                    PART_SelectionOuterEllipse.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        void ProcessMousedown(Point SelectionPoint)
        {
            this.SelectionPoint = SelectionPoint;

            var w = PART_ColorPlane.ActualWidth / 2;
            var h = PART_ColorPlane.ActualHeight / 2;
            this.SelectionTransform.X = (this.SelectionPoint.X - w).Coerce(w, -w);
            this.SelectionTransform.Y = (this.SelectionPoint.Y - h).Coerce(h, -h);

            this.SelectionPoint = new Point(this.SelectionPoint.X.Coerce(255.0), this.SelectionPoint.Y.Coerce(255.0));
            var NewColor = NormalComponent.ColorAtPoint(this.SelectionPoint, (int)PART_Slider.Value);
            if (!NormalComponent.IsNormalIndependantOfColor)
                NormalComponent.UpdateSlider(NormalPane, NewColor);

            this.Color = NewColor.WithAlpha(PART_AlphaSlider.Alpha);
        }

        public void IncrementNormalSlider()
        {
            PART_Slider.Value++;
        }

        #region Events

        void OnAlphaDisplayAlphaChanged(object sender, EventArgs<byte> e)
        {
            SetValue(ColorProperty, Color.WithAlpha(e.Value));
            if (ColorChanged != null)
                ColorChanged(this, new EventArgs<Color>(Color));
            this.Alpha = e.Value;
        }

        void OnColorPlaneMouseDown(object sender, MouseButtonEventArgs e)
        {
            ColorChangeSource = ColorChangeSourceType.MouseDown;
            ProcessMousedown(e.GetPosition((IInputElement)sender));
            ColorChangeSource = ColorChangeSourceType.ColorPropertySet;
        }

        void OnColorPlaneMouseMove(object sender, MouseEventArgs e)
        {
            ColorChangeSource = ColorChangeSourceType.MouseDown;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ((IInputElement)sender).CaptureMouse();
                var Position = e.GetPosition((IInputElement)sender);
                ProcessMousedown(Position);
            }
            ColorChangeSource = ColorChangeSourceType.ColorPropertySet;
        }

        void OnColorPlaneMouseUp(object sender, MouseEventArgs e)
        {
            ((IInputElement)sender).ReleaseMouseCapture();
        }

        void OnColorSliderMouseDown(object sender, MouseButtonEventArgs e)
        {
            var yPos = (e.GetPosition((IInputElement)sender)).Y;
            var proportion = 1 - yPos / 255;
            var componentRange = NormalComponent.MaxValue - NormalComponent.MinValue;
            var normalValue = NormalComponent.MinValue + proportion * componentRange;

            PART_Slider.Value = normalValue;
        }

        void OnColorSliderMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ((IInputElement)sender).CaptureMouse();
                var yPos = (e.GetPosition((IInputElement)sender)).Y;
                var proportion = 1 - yPos / 255;
                var componentRange = NormalComponent.MaxValue - NormalComponent.MinValue;

                var normalValue = NormalComponent.MinValue + proportion * componentRange;

                PART_Slider.Value = normalValue;
            }
        }

        void OnColorSliderMouseUp(object sender, MouseEventArgs e)
        {
            ((IInputElement)sender).ReleaseMouseCapture();
        }

        void OnColorSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ColorChangeSource = ColorChangeSourceType.SliderMove;
            if (ProcessSliderEvents)
            {
                ProcessSliderEvents = false;
                Color = NormalComponent.ColorAtPoint(SelectionPoint, (int)e.NewValue).WithAlpha(PART_AlphaSlider.Alpha);
                UpdateColorPlaneBitmap(NormalComponent.GetValue(Color));
                ProcessSliderEvents = true;
            }
            ColorChangeSource = ColorChangeSourceType.ColorPropertySet;
        }

        #endregion

        #endregion
    }
}
