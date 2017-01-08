using Imagin.Common.Extensions;
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
    /// A chip for displaying and selecting a brush.
    /// </summary>
    public abstract class ChipBase : UserControl
    {
        MouseEvent dialogEvent = MouseEvent.MouseDown;
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsSynchronizedProperty = DependencyProperty.Register("IsSynchronized", typeof(bool), typeof(ChipBase), new PropertyMetadata(true, OnIsSynchronizedChanged));
        /// <summary>
        /// 
        /// </summary>
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
            d.As<ChipBase>().OnIsSynchronizedChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ChipBase), new PropertyMetadata("Select Color"));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public ChipBase() : base()
        {
            DefaultStyleKey = typeof(ChipBase);
        }

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
        protected virtual void OnIsSynchronizedChanged(bool Value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool? ShowDialog();
    }
}
