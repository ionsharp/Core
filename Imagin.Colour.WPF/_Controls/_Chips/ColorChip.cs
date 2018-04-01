using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// A chip for displaying and selecting a <see cref="SolidColorBrush"/>.
    /// </summary>
    public class ColorChip : Chip<SolidColorBrush>
    {
        bool _ColorChangeHandled = false;

        bool _ValueChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Color>> ColorChanged;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorChip), new PropertyMetadata(Colors.Gray, new PropertyChangedCallback(OnColorChanged)));
        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        static void OnColorChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => ((ColorChip)element).OnColorChanged((Color)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public ColorChip() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnColorChanged(Color value)
        {
            if (!_ColorChangeHandled)
            {
                ColorChanged?.Invoke(this, new EventArgs<Color>(Color));

                _ValueChangeHandled = true;
                Value = new SolidColorBrush(value);
                _ValueChangeHandled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected override void OnValueChanged(SolidColorBrush value)
        {
            if (!_ValueChangeHandled && value != null)
            {
                base.OnValueChanged(value);

                _ColorChangeHandled = true;
                Color = value.Color;
                _ColorChangeHandled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool? ShowDialog()
        {
            if (Value == null)
                Value = Brushes.Transparent;

            var dialog = new ColorDialog(Title, Value, this);
            var result = dialog.ShowDialog();

            if (result != null)
                Value = dialog.IsSave ? dialog.Value : dialog.InitialValue;

            return result;
        }
    }
}