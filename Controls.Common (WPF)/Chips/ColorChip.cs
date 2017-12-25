using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A chip for displaying and selecting a SolidColorBrush.
    /// </summary>
    public class ColorChip : Chip<SolidColorBrush>
    {
        bool ColorChangeHandled = false;

        bool ValueChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<Color>> ColorChanged;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorChip), new PropertyMetadata(Colors.Gray, new PropertyChangedCallback(OnColorChanged)));
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
        static void OnColorChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((ColorChip)Object).OnColorChanged((Color)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public ColorChip() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnColorChanged(Color Value)
        {
            if (!ColorChangeHandled)
            {
                ColorChanged?.Invoke(this, new EventArgs<Color>(Color));

                ValueChangeHandled = true;
                this.Value = new SolidColorBrush(Value);
                ValueChangeHandled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected override void OnValueChanged(SolidColorBrush Value)
        {
            if (!ValueChangeHandled && Value != null)
            {
                base.OnValueChanged(Value);

                ColorChangeHandled = true;
                Color = Value.Color;
                ColorChangeHandled = false;
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

            var Dialog = new ColorDialog(Title, Value, this);
            var Result = Dialog.ShowDialog();

            if (Result != null)
                Value = Dialog.IsSave ? Dialog.Value : Dialog.InitialValue;

            return Result;
        }
    }
}
