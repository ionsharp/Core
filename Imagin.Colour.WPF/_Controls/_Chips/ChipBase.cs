using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// A chip for displaying and selecting a <see cref="Brush"/>.
    /// </summary>
    public abstract class ChipBase : Control
    {
        MouseEvent _dialogEvent = MouseEvent.MouseDown;
        /// <summary>
        /// 
        /// </summary>
        public MouseEvent DialogEvent
        {
            get => _dialogEvent;
            set => _dialogEvent = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsSynchronizedProperty = DependencyProperty.Register(nameof(IsSynchronized), typeof(bool), typeof(ChipBase), new PropertyMetadata(true, OnIsSynchronizedChanged));
        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized
        {
            get => (bool)GetValue(IsSynchronizedProperty);
            set => SetValue(IsSynchronizedProperty, value);
        }
        static void OnIsSynchronizedChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.To<ChipBase>().OnIsSynchronizedChanged((bool)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(ChipBase), new PropertyMetadata(string.Empty));
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public ChipBase() : base() => DefaultStyleKey = typeof(ChipBase);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (DialogEvent == MouseEvent.MouseDoubleClick)
            {
                ShowDialog();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (DialogEvent == MouseEvent.MouseDown)
            {
                ShowDialog();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (DialogEvent == MouseEvent.MouseUp)
            {
                ShowDialog();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnIsSynchronizedChanged(bool Value) { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool? ShowDialog();
    }
}