using Imagin.Common.Linq;
using Imagin.Common.Primitives;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Imagin.Common.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_DropDown", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_Options", Type = typeof(ListBox))]
    public class TimeUpDown : TimeSpanUpDown
    {
        #region Properties

        bool OptionSelectionHandled;

        Popup PART_DropDown;

        ListBox PART_Options;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<TimeSpan>> OptionSelected;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DropDownStyleProperty = DependencyProperty.Register("DropDownStyle", typeof(Style), typeof(TimeUpDown), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Style DropDownStyle
        {
            get
            {
                return (Style)GetValue(DropDownStyleProperty);
            }
            set
            {
                SetValue(DropDownStyleProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(TimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));
        /// <summary>
        /// 
        /// </summary>
        public bool IsDropDownOpen
        {
            get
            {
                return (bool)GetValue(IsDropDownOpenProperty);
            }
            set
            {
                SetValue(IsDropDownOpenProperty, value);
            }
        }
        static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<TimeUpDown>().OnIsDropDownOpenChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(TimeUpDown), new FrameworkPropertyMetadata(360d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double MaxDropDownHeight
        {
            get
            {
                return (double)GetValue(MaxDropDownHeightProperty);
            }
            set
            {
                SetValue(MaxDropDownHeightProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty StaysOpenProperty = DependencyProperty.Register("StaysOpen", typeof(bool), typeof(TimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool StaysOpen
        {
            get
            {
                return (bool)GetValue(StaysOpenProperty);
            }
            set
            {
                SetValue(StaysOpenProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty StaysOpenOnSelectionProperty = DependencyProperty.Register("StaysOpenOnSelection", typeof(bool), typeof(TimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool StaysOpenOnSelection
        {
            get
            {
                return (bool)GetValue(StaysOpenOnSelectionProperty);
            }
            set
            {
                SetValue(StaysOpenOnSelectionProperty, value);
            }
        }

        #endregion

        #region TimeUpDown

        /// <summary>
        /// 
        /// </summary>
        public TimeUpDown() : base()
        {
            DefaultStyleKey = typeof(TimeUpDown);
        }

        #endregion

        #region Methods

        void OnOptionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!OptionSelectionHandled)
            {
                var option = e.AddedItems.FirstOrDefault()?.As<DateTime>();

                if (option != null)
                {
                    var result = option.Value.TimeOfDay;
                    SetCurrentValue(ValueProperty.Property, result);

                    OptionSelectionHandled = true;
                    PART_Options.SelectedIndex = -1;
                    OptionSelectionHandled = false;

                    OptionSelected?.Invoke(this, new EventArgs<TimeSpan>(result));
                }

                if (!StaysOpenOnSelection)
                    SetCurrentValue(IsDropDownOpenProperty, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Options = Template.FindName("PART_Options", this) as ListBox;
            if (PART_Options != null)
            {
                var j = DateTime.MinValue.Date;
                for (var i = 0; i < 24; i++)
                {
                    PART_Options.Items.Add(j);
                    j = j.AddHours(1);
                }
                PART_Options.SelectionChanged += OnOptionSelected;
            }

            PART_DropDown = Template.FindName("PART_DropDown", this) as Popup;
            if (PART_DropDown != null)
                PART_DropDown.Closed += OnDropDownClosed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDropDownClosed(object sender, EventArgs e)
        {
            if (IsDropDownOpen)
                IsDropDownOpen = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnIsDropDownOpenChanged(bool Value)
        {
            if (PART_DropDown != null)
                PART_DropDown.IsOpen = Value;
        }

        #endregion
    }
}
