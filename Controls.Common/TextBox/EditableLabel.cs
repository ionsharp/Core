using Imagin.Common.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_Button", Type = typeof(MaskedButton))]
    public class EditableLabel : AdvancedTextBox
    {
        #region Properties

        MaskedButton PART_Button
        {
            get; set;
        }
        
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

        #endregion

        #region EditableLabel

        public EditableLabel() : base()
        {
            this.DefaultStyleKey = typeof(EditableLabel);
        }

        #endregion

        #region Methods

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (!this.IsEditable && this.MouseEvent == MouseEvent.MouseDoubleClick)
                OnEdited(true);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (!this.IsEditable && this.MouseEvent == MouseEvent.MouseDown)
                OnEdited(true);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (!this.IsEditable && this.MouseEvent == MouseEvent.MouseUp)
                OnEdited(true);
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

            this.PART_Button = this.Template.FindName("PART_Button", this) as MaskedButton;
            this.PART_Button.Click += (s, e) => OnEdited(true);
        }

        protected virtual void OnEdited(bool Value)
        {
            this.IsEditable = Value;
            if (Value) this.Focus();
        }

        #endregion
    }
}
