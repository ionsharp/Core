using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_Button", Type = typeof(MaskedButton))]
    public class EditableLabel : TextBoxExt
    {
        #region Properties

        int down = 0;

        MaskedButton PART_Button
        {
            get; set;
        }

        public event EventHandler<EventArgs<string>> Edited;

        public static DependencyProperty ButtonHorizontalAlignmentProperty = DependencyProperty.Register("ButtonHorizontalAlignment", typeof(HorizontalAlignment), typeof(EditableLabel), new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public HorizontalAlignment ButtonHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)GetValue(ButtonHorizontalAlignmentProperty);
            }
            set
            {
                SetValue(ButtonHorizontalAlignmentProperty, value);
            }
        }

        public static DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableLabel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsEditable
        {
            get
            {
                return (bool)GetValue(IsEditableProperty);
            }
            set
            {
                SetValue(IsEditableProperty, value);
            }
        }

        public static DependencyProperty MouseEventProperty = DependencyProperty.Register("MouseEvent", typeof(MouseEvent), typeof(EditableLabel), new FrameworkPropertyMetadata(MouseEvent.MouseUp, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public MouseEvent MouseEvent
        {
            get
            {
                return (MouseEvent)GetValue(MouseEventProperty);
            }
            set
            {
                SetValue(MouseEventProperty, value);
            }
        }

        public static DependencyProperty ShowButtonProperty = DependencyProperty.Register("ShowButton", typeof(bool), typeof(EditableLabel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowButton
        {
            get
            {
                return (bool)GetValue(ShowButtonProperty);
            }
            set
            {
                SetValue(ShowButtonProperty, value);
            }
        }

        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(EditableLabel), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TextTrimming TextTrimming
        {
            get
            {
                return (TextTrimming)GetValue(TextTrimmingProperty);
            }
            set
            {
                SetValue(TextTrimmingProperty, value);
            }
        }
        
        #endregion

        #region EditableLabel

        public EditableLabel() : base()
        {
            this.DefaultStyleKey = typeof(EditableLabel);
        }

        #endregion

        #region Methods

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDoubleClick(e);
            if (!IsEditable && MouseEvent == MouseEvent.MouseDoubleClick)
            {
                OnEdited(true);
                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            if (!IsEditable)
            {
                var d = false;
                switch (MouseEvent)
                {
                    case MouseEvent.DelayedMouseDown:
                        if (down == 1)
                        {
                            down = 0;
                            d = true;
                        }
                        else
                        {
                            down++;
                            e.Handled = true;
                        }
                        break;
                    case MouseEvent.MouseDown:
                        d = true;
                        break;
                }

                if (d)
                {
                    OnEdited(true);
                    e.Handled = true;
                }
            }
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (!IsEditable && MouseEvent == MouseEvent.MouseUp)
            {
                OnEdited(true);
                e.Handled = true;
            }
        }

        protected override void OnEntered(KeyEventArgs e)
        {
            base.OnEntered(e);
            OnEdited(false);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            OnEdited(false);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Button = this.Template.FindName("PART_Button", this) as MaskedButton;
            PART_Button.Click += (s, e) => OnEdited(true);
        }

        protected virtual void OnEdited(bool Value)
        {
            IsEditable = Value;

            if (Value)
            {
                Focus();
            }
            else Edited?.Invoke(this, new EventArgs<string>(Text));
        }

        #endregion
    }
}
