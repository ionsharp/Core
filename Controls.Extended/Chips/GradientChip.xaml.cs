using Imagin.Common.Events;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Imagin.Common.Extensions;
using Imagin.Common.Input;

namespace Imagin.Controls.Extended
{
    public partial class GradientChip : UserControl
    {
        #region Properties

        public event EventHandler<EventArgs<LinearGradientBrush>> GradientChanged;

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

        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(GradientChip), new PropertyMetadata("Edit Gradient"));
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

        public static DependencyProperty IsSynchronizedProperty = DependencyProperty.Register("IsSynchronized", typeof(bool), typeof(GradientChip), new PropertyMetadata(true));
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

        public static DependencyProperty GradientProperty = DependencyProperty.Register("Gradient", typeof(LinearGradientBrush), typeof(GradientChip), new PropertyMetadata(default(LinearGradientBrush), new PropertyChangedCallback(OnGradientChanged)));
        public LinearGradientBrush Gradient
        {
            get
            {
                return (LinearGradientBrush)GetValue(GradientProperty);
            }
            set
            {
                SetValue(GradientProperty, value);
            }
        }
        static void OnGradientChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ((GradientChip)Object).OnGradientChanged((LinearGradientBrush)e.NewValue);
        }

        #endregion

        #endregion

        #region GradientChip

        public GradientChip()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        void OnGradientChanged(LinearGradientBrush LinearGradientBrush)
        {
            if (this.GradientChanged != null)
                this.GradientChanged(this, new EventArgs<LinearGradientBrush>(LinearGradientBrush));
        }

        public virtual bool? ShowDialog()
        {
            var InitialGradient = this.Gradient.Duplicate();
            var Dialog = new GradientDialog(this.Title, InitialGradient, this);
            var Result = Dialog.ShowDialog();
            if (Result.Value || Dialog.Cancel)
                this.Gradient = InitialGradient;
            else if (!this.IsSynchronized)
                this.Gradient = Dialog.Gradient;
            return Result;
        }

        #region Events

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DialogEvent != MouseEvent.MouseDown) return;
            this.ShowDialog();
            e.Handled = true;
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.DialogEvent != MouseEvent.MouseUp) return;
            this.ShowDialog();
            e.Handled = true;
        }

        void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.DialogEvent != MouseEvent.MouseDoubleClick) return;
            this.ShowDialog();
            e.Handled = true;
        }

        #endregion

        #endregion
    }
}
