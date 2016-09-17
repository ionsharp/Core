using Imagin.Common.Events;
using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class ColorChip : UserControl
    {
        #region Properties

        bool Switch = false;

        public event EventHandler<EventArgs<Color>> ColorChanged;

        MouseEvent dialogEvent = MouseEvent.MouseDown;
        public MouseEvent DialogEvent
        {
            get
            {
                return dialogEvent;
            }
            set
            {
                dialogEvent = value;
            }
        }

        #region Dependency
        
        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ColorChip), new PropertyMetadata("Select Color"));
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static DependencyProperty IsSynchronizedProperty = DependencyProperty.Register("IsSynchronized", typeof(bool), typeof(ColorChip), new PropertyMetadata(true, new PropertyChangedCallback(OnIsSynchronizedChanged)));
        public bool IsSynchronized
        {
            get
            {
                return (bool)GetValue(IsSynchronizedProperty);
            }
            set
            {
                SetValue(IsSynchronizedProperty, value);
            }
        }
        static void OnIsSynchronizedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorChip), new PropertyMetadata(Colors.Gray, new PropertyChangedCallback(OnColorChanged)));
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
        static void OnColorChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((ColorChip)Object).OnColorChanged((Color)e.NewValue);
        }
        void OnColorChanged(Color Color)
        {
            if (this.Switch)
            {
                this.Switch = false;
                return;
            }
            this.Switch = true;
            this.Brush = new SolidColorBrush(Color);
            if (this.ColorChanged != null)
                this.ColorChanged(this, new EventArgs<Color>(Color));
        }

        public static DependencyProperty BrushProperty = DependencyProperty.Register("Brush", typeof(SolidColorBrush), typeof(ColorChip), new PropertyMetadata(null, OnBrushChanged));
        public SolidColorBrush Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(BrushProperty);
            }
            set
            {
                SetValue(BrushProperty, value);
            }
        }
        static void OnBrushChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((ColorChip)Object).OnBrushChanged((SolidColorBrush)e.NewValue);
        }
        void OnBrushChanged(SolidColorBrush Brush)
        {
            if (this.Switch)
            {
                this.Switch = false;
                return;
            }
            this.Switch = true;
            if (Brush != null)
                this.Color = Brush.Color;
        }

        #endregion

        #endregion

        #region ColorChip

        public ColorChip()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public virtual bool? ShowDialog()
        {
            var InitialColor = this.Color;
            var Dialog = new ColorDialog(this.Title, InitialColor, this);
            var Result = Dialog.ShowDialog();
            if (Result.Value || Dialog.Cancel)
                this.Brush = new SolidColorBrush(InitialColor);
            else if (!this.IsSynchronized)
                this.Brush = new SolidColorBrush(Dialog.SelectedColor);
            return Result;
        }

        #region Events

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DialogEvent == MouseEvent.MouseDown)
            {
                this.ShowDialog();
                e.Handled = true;
            }
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.DialogEvent == MouseEvent.MouseUp)
            {
                this.ShowDialog();
                e.Handled = true;
            }
        }

        void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.DialogEvent == MouseEvent.MouseDoubleClick)
            {
                this.ShowDialog();
                e.Handled = true;
            }
        }

        #endregion

        #endregion
    }
}
