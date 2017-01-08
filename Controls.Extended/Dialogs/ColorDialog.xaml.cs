using Imagin.Common.Input;
using Imagin.Controls.Common;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class ColorDialog : BasicWindow, IColorDialog
    {
        #region Properties

        public Color SelectedColor
        {
            get
            {
                return PART_ColorPicker.SelectedColor;
            }
            set
            {
                PART_ColorPicker.SelectedColor = value;
            }
        }

        public Color InitialColor
        {
            get
            {
                return PART_ColorPicker.InitialColor;
            }
            set
            {
                PART_ColorPicker.InitialColor = value;
                PART_ColorPicker.SelectedColor = value;
            }
        }

        public ColorSelector.SelectionRingType SelectionRing
        {
            get
            {
                return PART_ColorPicker.SelectionRing;
            }
            set
            {
                PART_ColorPicker.SelectionRing = value;
            }
        }

        public WindowResult Result = WindowResult.Unknown;

        public ColorChip ColorChip
        {
            get; set;
        }

        #endregion

        #region ColorDialog

        public ColorDialog() : base()
        {
            InitializeComponent();
            PART_ColorPicker.SelectedColorChanged += OnSelectedColorChanged;
        }

        public ColorDialog(string title, Color initialColor, ColorChip colorChip = null) : this()
        {
            Title = title;
            InitialColor = initialColor;
            ColorChip = colorChip;
        }

        #endregion

        #region Events
        
        void OnCancel(object sender, RoutedEventArgs e)
        {
            Result = WindowResult.Cancel;
            Close();
        }

        void OnOk(object sender, RoutedEventArgs e)
        {
            Result = WindowResult.Ok;
            Close();
        }

        void OnRevert(object sender, RoutedEventArgs e)
        {
            SelectedColor = InitialColor;
        }

        void OnSelectedColorChanged(object sender, EventArgs<Color> e)
        {
            if (ColorChip != null && ColorChip.IsSynchronized)
                ColorChip.Color = e.Value;
        }

        #endregion
    }
}
