using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class TimeUpDown : UpDown<DateTime>
    {
        #region Properties

        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(TimeSpan), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(default(TimeSpan), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TimeSpan Increment
        {
            get
            {
                return (TimeSpan)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        #endregion

        #region TimeUpDown

        public TimeUpDown() : base()
        {
            this.DefaultStyleKey = typeof(TimeUpDown);

            this.Increment = TimeSpan.FromDays(1.0);
            this.Minimum = DateTime.MinValue;
            this.Maximum = DateTime.MaxValue;
            this.Value = DateTime.Now;
        }

        #endregion

        #region Methods

        protected override bool CanIncrease()
        {
            return this.Value < this.Maximum;
        }

        protected override bool CanDecrease()
        {
            return this.Value > this.Minimum;
        }

        protected override object OnMaximumCoerced(object NewMaximum)
        {
            return NewMaximum.As<DateTime>() < this.Minimum ? this.Minimum : (NewMaximum.As<long>() > long.MaxValue ? long.MaxValue : NewMaximum);
        }

        protected override object OnMinimumCoerced(object NewMinimum)
        {
            return NewMinimum.As<DateTime>() > this.Maximum ? this.Maximum : (NewMinimum.As<long>() < long.MinValue ? long.MinValue : NewMinimum);
        }

        protected override bool OnTextChanged()
        {
            if (!base.OnTextChanged()) return false;
            this.Value = this.OnValueCoerced(this.Text.ToDateTime()).As<DateTime>();
            return true;
        }

        protected override bool OnValueChanged()
        {
            if (!base.OnValueChanged()) return false;
            this.SetText(this.Value.ToString(this.StringFormat));
            return true;
        }

        protected override object OnValueCoerced(object NewValue)
        {
            return NewValue.As<DateTime>() < this.Minimum ? this.Minimum : (NewValue.As<DateTime>() > this.Maximum ? this.Maximum : NewValue);
        }

        public override void Decrease()
        {
            this.Value -= this.Increment;
        }

        public override void Increase()
        {
            this.Value += this.Increment;
        }

        #endregion
    }
}
