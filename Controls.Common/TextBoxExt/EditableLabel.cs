using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Button", Type = typeof(MaskedButton))]
    public class EditableLabel : TextBoxExt
    {
        #region Properties

        int down = 0;

        MaskedButton PART_Button
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<string>> Edited;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ButtonHorizontalAlignmentProperty = DependencyProperty.Register("ButtonHorizontalAlignment", typeof(HorizontalAlignment), typeof(EditableLabel), new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableLabel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MouseEventProperty = DependencyProperty.Register("MouseEvent", typeof(MouseEvent), typeof(EditableLabel), new FrameworkPropertyMetadata(MouseEvent.MouseUp, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowButtonProperty = DependencyProperty.Register("ShowButton", typeof(bool), typeof(EditableLabel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(EditableLabel), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public EditableLabel() : base()
        {
            this.DefaultStyleKey = typeof(EditableLabel);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDoubleClick(e);
            if (!IsEditable && MouseEvent == MouseEvent.MouseDoubleClick)
            {
                OnEdited(true);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (!IsEditable && MouseEvent == MouseEvent.MouseUp)
            {
                OnEdited(true);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEntered(KeyEventArgs e)
        {
            base.OnEntered(e);
            OnEdited(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            OnEdited(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Button = this.Template.FindName("PART_Button", this) as MaskedButton;
            PART_Button.Click += (s, e) => OnEdited(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
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
