using Imagin.Colour.Controls.Collections;
using Imagin.Colour.Controls.Models;
using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ColorPicker : UserControl, IBrushPicker<SolidColorBrush>
    {
        #region Properties

        byte? _lastAlpha = null;

        double? _lastInputWidth = null;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Color>> SelectedColorChanged;

        /// <summary>
        /// 
        /// </summary>
        public readonly static ColorModel[] DefaultColorModels = new ColorModel[]
        {
            new RGBViewModel(),
            new HSBViewModel(),
            new HSLViewModel(),
            new HunterLabViewModel(),
            new LabViewModel(),
            new LChabViewModel(),
            new LChuvViewModel(),
            new LMSViewModel(),
            new LuvViewModel(),
            new xyYViewModel(),
            new XYZViewModel(),
            new YUVViewModel(),
            new CMYKViewModel(),
        };

        Imagin.Colour.Conversion.ColorConverter _converter = new Imagin.Colour.Conversion.ColorConverter();
        /// <summary>
        /// 
        /// </summary>
        public Imagin.Colour.Conversion.ColorConverter Converter => _converter;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(ColorPicker), new FrameworkPropertyMetadata((byte)255, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public byte Alpha
        {
            get => (byte)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CanUpDownComponentsProperty = DependencyProperty.Register("CanUpDownComponents", typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool CanUpDownComponents
        {
            get => (bool)GetValue(CanUpDownComponentsProperty);
            set => SetValue(CanUpDownComponentsProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerBackgroundProperty = DependencyProperty.Register("CheckerBackground", typeof(SolidColorBrush), typeof(ColorPicker), new FrameworkPropertyMetadata(Brushes.White));
        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrush CheckerBackground
        {
            get => (SolidColorBrush)GetValue(CheckerBackgroundProperty);
            set => SetValue(CheckerBackgroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerForegroundProperty = DependencyProperty.Register("CheckerForeground", typeof(SolidColorBrush), typeof(ColorPicker), new FrameworkPropertyMetadata(Brushes.LightGray));
        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrush CheckerForeground
        {
            get => (SolidColorBrush)GetValue(CheckerForegroundProperty);
            set => SetValue(CheckerForegroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ComponentStringFormatProperty = DependencyProperty.Register("ComponentStringFormat", typeof(string), typeof(ColorPicker), new FrameworkPropertyMetadata("N0", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string ComponentStringFormat
        {
            get => (string)GetValue(ComponentStringFormatProperty);
            set => SetValue(ComponentStringFormatProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ComponentWidthProperty = DependencyProperty.Register("ComponentWidth", typeof(double), typeof(ColorPicker), new FrameworkPropertyMetadata(65.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double ComponentWidth
        {
            get => (double)GetValue(ComponentWidthProperty);
            set => SetValue(ComponentWidthProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty InputVisibilityProperty = DependencyProperty.Register("InputVisibility", typeof(Visibility), typeof(ColorPicker), new FrameworkPropertyMetadata(Visibility.Visible, OnInputVisibilityChanged));
        /// <summary>
        /// 
        /// </summary>
        public Visibility InputVisibility
        {
            get => (Visibility)GetValue(InputVisibilityProperty);
            set => SetValue(InputVisibilityProperty, value);
        }
        static void OnInputVisibilityChanged(DependencyObject element, DependencyPropertyChangedEventArgs e) => element.As<ColorPicker>().OnInputVisibilityChanged((Visibility)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty InputWidthProperty = DependencyProperty.Register("InputWidth", typeof(double), typeof(ColorPicker), new FrameworkPropertyMetadata(420.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double InputWidth
        {
            get => (double)GetValue(InputWidthProperty);
            set => SetValue(InputWidthProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsAsyncProperty = DependencyProperty.Register("IsAsync", typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not to use asynchronous bindings.
        /// </summary>
        public bool IsAsync
        {
            get => (bool)GetValue(IsAsyncProperty);
            set => SetValue(IsAsyncProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorModelsProperty = DependencyProperty.Register("ColorModels", typeof(ColorModelCollection), typeof(ColorPicker), new FrameworkPropertyMetadata(default(ColorModelCollection), OnColorModelsChanged));
        /// <summary>
        /// 
        /// </summary>
        public ColorModelCollection ColorModels
        {
            get => (ColorModelCollection)GetValue(ColorModelsProperty);
            set => SetValue(ColorModelsProperty, value);
        }
        static void OnColorModelsChanged(DependencyObject element, DependencyPropertyChangedEventArgs e) => element.As<ColorPicker>().OnColorModelsChanged((ColorModelCollection)e.OldValue, (ColorModelCollection)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NewColorProperty = DependencyProperty.Register("NewColor", typeof(Color), typeof(ColorPicker), new FrameworkPropertyMetadata(Colors.Transparent, OnNewColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color NewColor
        {
            get => (Color)GetValue(NewColorProperty);
            set => SetValue(NewColorProperty, value);
        }
        static void OnNewColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e) => element.As<ColorPicker>().OnNewColorChanged((Color)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OldColorProperty = DependencyProperty.Register("OldColor", typeof(Color), typeof(ColorPicker), new FrameworkPropertyMetadata(Colors.Transparent, OnOldColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color OldColor
        {
            get => (Color)GetValue(OldColorProperty);
            set => SetValue(OldColorProperty, value);
        }
        static void OnOldColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e) => element.As<ColorPicker>().OnOldColorChanged((Color)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedComponentProperty = DependencyProperty.Register("SelectedComponent", typeof(VisualComponent), typeof(ColorPicker), new FrameworkPropertyMetadata(default(VisualComponent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public VisualComponent SelectedComponent
        {
            get => (VisualComponent)GetValue(SelectedComponentProperty);
            private set => SetValue(SelectedComponentProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedColorModelProperty = DependencyProperty.Register("SelectedColorModel", typeof(ColorModel), typeof(ColorPicker), new FrameworkPropertyMetadata(default(ColorModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ColorModel SelectedColorModel
        {
            get => (ColorModel)GetValue(SelectedColorModelProperty);
            private set => SetValue(SelectedColorModelProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionLengthProperty = DependencyProperty.Register("SelectionLength", typeof(double), typeof(ColorPicker), new PropertyMetadata(12.0));
        /// <summary>
        /// 
        /// </summary>
        public double SelectionLength
        {
            get => (double)GetValue(SelectionLengthProperty);
            set => SetValue(SelectionLengthProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionTypeProperty = DependencyProperty.Register("SelectionType", typeof(ColorSelectionType), typeof(ColorPicker), new PropertyMetadata(ColorSelectionType.BlackAndWhite));
        /// <summary>
        /// 
        /// </summary>
        public ColorSelectionType SelectionType
        {
            get => (ColorSelectionType)GetValue(SelectionTypeProperty);
            set => SetValue(SelectionTypeProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TransparencyProperty = DependencyProperty.Register("Transparency", typeof(Transparency), typeof(ColorPicker), new FrameworkPropertyMetadata(Transparency.Transparent, OnTransparencyChanged));
        /// <summary>
        /// 
        /// </summary>
        public Transparency Transparency
        {
            get => (Transparency)GetValue(TransparencyProperty);
            set => SetValue(TransparencyProperty, value);
        }
        static void OnTransparencyChanged(DependencyObject element, DependencyPropertyChangedEventArgs e) => element.As<ColorPicker>().OnTransparencyChanged((Transparency)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty WorkingSpaceProperty = DependencyProperty.Register("WorkingSpace", typeof(WorkingSpace), typeof(ColorPicker), new FrameworkPropertyMetadata(WorkingSpaces.Default));
        /// <summary>
        /// 
        /// </summary>
        public WorkingSpace WorkingSpace
        {
            get => (WorkingSpace)GetValue(WorkingSpaceProperty);
            set => SetValue(WorkingSpaceProperty, value);
        }

        #endregion

        #region ColorPicker

        /// <summary>
        /// 
        /// </summary>
        public ColorPicker()
        {
            SetCurrentValue(ColorModelsProperty, new ColorModelCollection());
            InitializeComponent();
        }

        #endregion

        #region Methods

        void OnColorModelSelected(object sender, SelectedEventArgs e)
        {
            SetCurrentValue(SelectedColorModelProperty, e.Value);
            SetCurrentValue(SelectedComponentProperty, e.Parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnColorModelsChanged(ColorModelCollection OldValue, ColorModelCollection NewValue)
        {
            if (OldValue != null)
                OldValue.Selected -= OnColorModelSelected;

            if (NewValue != null)
            {
                NewValue.Selected -= OnColorModelSelected;
                NewValue.Selected += OnColorModelSelected;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnInputVisibilityChanged(Visibility Value)
        {
            if (Value == Visibility.Collapsed)
            {
                _lastInputWidth = InputWidth;
                InputWidth = 0d;
            }
            else if (InputWidth == 0 && _lastInputWidth != null)
            {
                InputWidth = _lastInputWidth.Value;
                _lastInputWidth = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnNewColorChanged(Color value) => SelectedColorChanged?.Invoke(this, new EventArgs<Color>(value));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnOldColorChanged(Color value) => PART_OldRectangle.Fill = new SolidColorBrush(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnTransparencyChanged(Transparency value)
        {
            if (value == Transparency.Transparent)
            {
                if (_lastAlpha != null)
                {
                    Alpha = _lastAlpha.Value;
                    _lastAlpha = null;
                }
            }
            else if (value == Transparency.Opaque)
            {
                _lastAlpha = Alpha;
                Alpha = 255;
            }
        }

        #endregion
    }
}
