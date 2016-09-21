using Imagin.Common.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_Button", Type = typeof(MaskedButton))]
    [TemplatePart(Name = "PART_TextBox", Type = typeof(AdvancedTextBox))]
    public class EditableLabel : Label
    {
        MaskedButton PART_Button
        {
            get; set;
        }

        AdvancedTextBox PART_TextBox
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

        public static DependencyProperty IsEnterEnabledProperty = DependencyProperty.Register("IsEnterEnabled", typeof(bool), typeof(EditableLabel), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsEnterEnabled
        {
            get
            {
                return (bool)GetValue(IsEnterEnabledProperty);
            }
            set
            {
                SetValue(IsEnterEnabledProperty, value);
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

        public EditableLabel() : base()
        {
            this.DefaultStyleKey = typeof(EditableLabel);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            this.SetBinding();
        }

        protected override void OnContentStringFormatChanged(string oldContentStringFormat, string newContentStringFormat)
        {
            base.OnContentStringFormatChanged(oldContentStringFormat, newContentStringFormat);
            this.SetBinding();
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (!this.IsEditable && this.MouseEvent == MouseEvent.MouseDoubleClick)
                SetIsEditable(true);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (!this.IsEditable && this.MouseEvent == MouseEvent.MouseDown)
                SetIsEditable(true);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (!this.IsEditable && this.MouseEvent == MouseEvent.MouseUp)
                SetIsEditable(true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_TextBox = this.Template.FindName("PART_TextBox", this) as AdvancedTextBox;
            this.PART_TextBox.KeyDown += (s, e) =>
            {
                if (this.IsEnterEnabled && e.Key == Key.Enter)
                    this.IsEditable = false;
            };
            this.PART_TextBox.LostFocus += (s, e) => this.IsEditable = false;

            this.PART_Button = this.Template.FindName("PART_Button", this) as MaskedButton;
            this.PART_Button.Click += (s, e) => this.IsEditable = true;

            this.SetBinding();
        }

        void SetBinding()
        {
            if (this.PART_TextBox == null) return;
            var Result = new Binding()
            {
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath("Content"),
                Source = this,
                StringFormat = this.ContentStringFormat
            };
            BindingOperations.SetBinding(this.PART_TextBox, AdvancedTextBox.TextProperty, Result);
        }

        void SetIsEditable(bool Value)
        {
            this.IsEditable = true;
            if (this.IsEditable)
                this.PART_TextBox.Focus();
        }
    }
}
