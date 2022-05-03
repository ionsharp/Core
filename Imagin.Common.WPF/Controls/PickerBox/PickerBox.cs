using Imagin.Common.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public abstract class PickerBox : Control
    {
        MouseEvent _dialogEvent = MouseEvent.MouseDown;
        public MouseEvent DialogEvent
        {
            get => _dialogEvent;
            set => _dialogEvent = value;
        }

        public static readonly DependencyProperty InnerBorderBrushProperty = DependencyProperty.Register(nameof(InnerBorderBrush), typeof(Brush), typeof(PickerBox), new FrameworkPropertyMetadata(default(Brush)));
        public Brush InnerBorderBrush
        {
            get => (Brush)GetValue(InnerBorderBrushProperty);
            set => SetValue(InnerBorderBrushProperty, value);
        }

        public static readonly DependencyProperty InnerBorderThicknessProperty = DependencyProperty.Register(nameof(InnerBorderThickness), typeof(Thickness), typeof(PickerBox), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness InnerBorderThickness
        {
            get => (Thickness)GetValue(InnerBorderThicknessProperty);
            set => SetValue(InnerBorderThicknessProperty, value);
        }
        
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(PickerBox), new FrameworkPropertyMetadata(string.Empty));
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty ValueTemplateProperty = DependencyProperty.Register(nameof(ValueTemplate), typeof(DataTemplate), typeof(PickerBox), new FrameworkPropertyMetadata(null));
        public DataTemplate ValueTemplate
        {
            get => (DataTemplate)GetValue(ValueTemplateProperty);
            set => SetValue(ValueTemplateProperty, value);
        }

        public PickerBox() : base() { }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (DialogEvent == MouseEvent.MouseDoubleClick)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    ShowDialog();
                    e.Handled = true;
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (DialogEvent == MouseEvent.MouseDown)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    ShowDialog();
                    e.Handled = true;
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (DialogEvent == MouseEvent.MouseUp)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    ShowDialog();
                    e.Handled = true;
                }
            }
        }

        public abstract bool? ShowDialog();
    }
}