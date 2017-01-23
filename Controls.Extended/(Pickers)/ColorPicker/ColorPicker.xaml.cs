using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// A color picker inspired by http://www.codeproject.com/Articles/131708/WPF-Color-Picker-Construction-Kit.
    /// </summary>
    public partial class ColorPicker : UserControl, IBrushPicker<SolidColorBrush>
    {
        #region Properties

        byte? LastAlpha = null;

        double? LastComponentsWidth = null;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Color>> SelectedColorChanged;

        /// <summary>
        /// 
        /// </summary>
        public ColorSelector.SelectionRingType SelectionRing
        {
            get
            {
                return this.PART_ColorSelector.SelectionRing;
            }
            set
            {
                this.PART_ColorSelector.SelectionRing = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(ColorPicker), new FrameworkPropertyMetadata((byte)255, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ComponentsWidthProperty = DependencyProperty.Register("ComponentsWidth", typeof(double), typeof(ColorPicker), new FrameworkPropertyMetadata(425.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double ComponentsWidth
        {
            get
            {
                return (double)GetValue(ComponentsWidthProperty);
            }
            set
            {
                SetValue(ComponentsWidthProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty InitialColorProperty = DependencyProperty.Register("InitialColor", typeof(Color), typeof(ColorPicker), new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnInitialColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color InitialColor
        {
            get
            {
                return (Color)GetValue(InitialColorProperty);
            }
            set
            {
                SetValue(InitialColorProperty, value);
            }
        }
        static void OnInitialColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorPicker>().OnInitialColorChanged((Color)e.NewValue);
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
            get
            {
                return (bool)GetValue(IsAsyncProperty);
            }
            set
            {
                SetValue(IsAsyncProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ModelsProperty = DependencyProperty.Register("Models", typeof(ObservableCollection<ColorSpaceModel>), typeof(ColorPicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ColorSpaceModel> Models
        {
            get
            {
                return (ObservableCollection<ColorSpaceModel>)GetValue(ModelsProperty);
            }
            private set
            {
                SetValue(ModelsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker), new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue(SelectedColorProperty);
            }
            set
            {
                SetValue(SelectedColorProperty, value);
            }
        }
        static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorPicker>().OnSelectedColorChanged((Color)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowComponentsProperty = DependencyProperty.Register("ShowComponents", typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnShowComponentsChanged));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowComponents
        {
            get
            {
                return (bool)GetValue(ShowComponentsProperty);
            }
            set
            {
                SetValue(ShowComponentsProperty, value);
            }
        }
        static void OnShowComponentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorPicker>().OnShowComponentsChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowNewCurrentProperty = DependencyProperty.Register("ShowNewCurrent", typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowNewCurrent
        {
            get
            {
                return (bool)GetValue(ShowNewCurrentProperty);
            }
            set
            {
                SetValue(ShowNewCurrentProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowSliderProperty = DependencyProperty.Register("ShowSlider", typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ShowAlphaProperty = DependencyProperty.Register("ShowAlpha", typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnShowAlphaChanged));
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
        static void OnShowAlphaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ColorPicker>().OnShowAlphaChanged((bool)e.NewValue);
        }

        #endregion

        #region ColorPicker

        /// <summary>
        /// 
        /// </summary>
        public ColorPicker()
        {
            Models = new ObservableCollection<ColorSpaceModel>();
            Models.Add(new RgbModel());
            Models.Add(new HsbModel());
            Models.Add(new HslModel());
            Models.Add(new XyzModel());
            Models.Add(new LabModel());
            Models.Add(new LchModel());
            //Models.Add(new LuvModel());
            //Models.Add(new CmykModel());

            InitializeComponent();

            LayoutUpdated += OnLayoutUpdated;
        }

        #endregion

        #region Methods

        void OnComponentChecked(object sender, RoutedEventArgs e)
        {
            PART_ColorSelector.NormalComponent = sender.As<FrameworkElement>().Tag.As<NormalComponentModel>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnInitialColorChanged(Color Value)
        {
            newCurrent.CurrentColor = Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLayoutUpdated(object sender, EventArgs e)
        {
            if (!IsLoaded || PART_ColorSelector.NormalComponent != null) return;
            if (PART_ColorSelector.NormalComponent == null)
                Models.Where(x => x.Is<HsbModel>()).First().Components[typeof(HsbModel.HComponent)].As<NormalComponentModel>().IsEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSelectedColorChanged(Color Value)
        {
            SelectedColorChanged?.Invoke(this, new EventArgs<Color>(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnShowAlphaChanged(bool Value)
        {
            if (Value)
            {
                if (LastAlpha != null)
                {
                    Alpha = LastAlpha.Value;
                    LastAlpha = null;
                }
            }
            else
            {
                LastAlpha = Alpha;
                Alpha = 255;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnShowComponentsChanged(bool Value)
        {
            if (!Value)
            {
                LastComponentsWidth = ComponentsWidth;
                ComponentsWidth = 0d;
            }
            else if (ComponentsWidth == 0 && LastComponentsWidth != null)
            {
                ComponentsWidth = LastComponentsWidth.Value;
                LastComponentsWidth = null;
            }
        }

        #endregion
    }
}
