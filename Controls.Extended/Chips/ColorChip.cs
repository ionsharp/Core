using Imagin.Common.Input;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Extended
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
        public ColorChip()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnColorChanged(Color Value)
        {
            if (ColorChanged != null)
                ColorChanged(this, new EventArgs<Color>(Color));

            if (!ColorChangeHandled)
            {
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
            base.OnValueChanged(Value);

            if (!ValueChangeHandled && Value != null)
            {
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

            var Dialog = new ColorDialog(Title, Value.Color, this);
            var Result = Dialog.ShowDialog();

            if (Result != null)
                Value = new SolidColorBrush(Dialog.Result == Common.WindowResult.Ok ? Dialog.SelectedColor : Dialog.InitialColor);

            return Result;
        }
    }
}
