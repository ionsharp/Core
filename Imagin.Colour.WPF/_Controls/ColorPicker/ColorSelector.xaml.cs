using Imagin.Colour.Controls.Models;
using Imagin.Common;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class ColorSelector : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(ColorSelector), new FrameworkPropertyMetadata((byte)255, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty CheckerBackgroundProperty = DependencyProperty.Register("CheckerBackground", typeof(SolidColorBrush), typeof(ColorSelector), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrush CheckerBackground
        {
            get
            {
                return (SolidColorBrush)GetValue(CheckerBackgroundProperty);
            }
            set
            {
                SetValue(CheckerBackgroundProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerForegroundProperty = DependencyProperty.Register("CheckerForeground", typeof(SolidColorBrush), typeof(ColorSelector), new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrush CheckerForeground
        {
            get
            {
                return (SolidColorBrush)GetValue(CheckerForegroundProperty);
            }
            set
            {
                SetValue(CheckerForegroundProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorSelector), new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ComponentProperty = DependencyProperty.Register("Component", typeof(VisualComponent), typeof(ColorSelector), new FrameworkPropertyMetadata(default(VisualComponent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public VisualComponent Component
        {
            get
            {
                return (VisualComponent)GetValue(ComponentProperty);
            }
            set
            {
                SetValue(ComponentProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ComponentValueProperty = DependencyProperty.Register("ComponentValue", typeof(double), typeof(ColorSelector), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double ComponentValue
        {
            get
            {
                return (double)GetValue(ComponentValueProperty);
            }
            set
            {
                SetValue(ComponentValueProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TransparencyProperty = DependencyProperty.Register("Transparency", typeof(Transparency), typeof(ColorSelector), new FrameworkPropertyMetadata(Transparency.Transparent));
        /// <summary>
        /// 
        /// </summary>
        public Transparency Transparency
        {
            get => (Transparency)GetValue(TransparencyProperty);
            set => SetValue(TransparencyProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(ColorModel), typeof(ColorSelector), new FrameworkPropertyMetadata(default(ColorModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ColorModel Model
        {
            get
            {
                return (ColorModel)GetValue(ModelProperty);
            }
            set
            {
                SetValue(ModelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionLengthProperty = DependencyProperty.Register("SelectionLength", typeof(double), typeof(ColorSelector), new PropertyMetadata(12.0));
        /// <summary>
        /// 
        /// </summary>
        public double SelectionLength
        {
            get
            {
                return (double)GetValue(SelectionLengthProperty);
            }
            set
            {
                SetValue(SelectionLengthProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectionTypeProperty = DependencyProperty.Register("SelectionType", typeof(ColorSelectionType), typeof(ColorSelector), new PropertyMetadata(ColorSelectionType.BlackAndWhite));
        /// <summary>
        /// 
        /// </summary>
        public ColorSelectionType SelectionType
        {
            get
            {
                return (ColorSelectionType)GetValue(SelectionTypeProperty);
            }
            set
            {
                SetValue(SelectionTypeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ColorSelector() : base()
        {
            InitializeComponent();
        }
    }
}