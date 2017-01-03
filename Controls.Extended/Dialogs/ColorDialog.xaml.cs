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

        public bool Save = false;

        public bool Cancel = false;

        public ColorChip ColorChip
        {
            get; set;
        }

        #endregion

        #region ColorDialog

        public ColorDialog()
        {
            InitializeComponent();
            this.PART_ColorPicker.SelectedColorChanged += OnSelectedColorChanged;
        }

        public ColorDialog(string Title, Color InitialColor, ColorChip ColorChip = null)
        {
            InitializeComponent();
            this.Title = Title;
            this.InitialColor = InitialColor;
            this.ColorChip = ColorChip;
            this.PART_ColorPicker.SelectedColorChanged += OnSelectedColorChanged;
        }

        #endregion

        #region Events
        
        void OnCancel(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = this.InitialColor;
            this.Cancel = true;
            this.Close();
        }

        void OnOk(object sender, RoutedEventArgs e)
        {
            this.Save = true;
            this.Close();
        }

        void OnRevert(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = this.InitialColor;
        }

        void OnSelectedColorChanged(object sender, EventArgs<Color> e)
        {
            if (ColorChip != null && ColorChip.IsSynchronized)
                ColorChip.Color = e.Value;
        }

        #endregion
    }
}
