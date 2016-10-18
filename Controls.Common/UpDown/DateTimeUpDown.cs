using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_Calendar", Type = typeof(Calendar))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class DateTimeUpDown : TimeUpDown
    {
        #region Properties

        Calendar PART_Calendar
        {
            get; set;
        }

        Popup PART_Popup
        {
            get; set;
        }

        public static DependencyProperty HidePopupOnMouseDoubleClickProperty = DependencyProperty.Register("HidePopupOnMouseDoubleClick", typeof(bool), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool HidePopupOnMouseDoubleClick
        {
            get
            {
                return (bool)GetValue(HidePopupOnMouseDoubleClickProperty);
            }
            set
            {
                SetValue(HidePopupOnMouseDoubleClickProperty, value);
            }
        }

        public static DependencyProperty IsPopupOpenProperty = DependencyProperty.Register("IsPopupOpen", typeof(bool), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsPopupOpen
        {
            get
            {
                return (bool)GetValue(IsPopupOpenProperty);
            }
            set
            {
                SetValue(IsPopupOpenProperty, value);
            }
        }

        public static DependencyProperty PopupAnimationProperty = DependencyProperty.Register("PopupAnimation", typeof(PopupAnimation), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(PopupAnimation.Fade, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public PopupAnimation PopupAnimation
        {
            get
            {
                return (PopupAnimation)GetValue(PopupAnimationProperty);
            }
            set
            {
                SetValue(PopupAnimationProperty, value);
            }
        }

        public static DependencyProperty PopupBackgroundProperty = DependencyProperty.Register("PopupBackground", typeof(Brush), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush PopupBackground
        {
            get
            {
                return (Brush)GetValue(PopupBackgroundProperty);
            }
            set
            {
                SetValue(PopupBackgroundProperty, value);
            }
        }

        public static DependencyProperty PopupBorderBrushProperty = DependencyProperty.Register("PopupBorderBrush", typeof(Brush), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush PopupBorderBrush
        {
            get
            {
                return (Brush)GetValue(PopupBorderBrushProperty);
            }
            set
            {
                SetValue(PopupBorderBrushProperty, value);
            }
        }

        public static DependencyProperty PopupBorderThicknessProperty = DependencyProperty.Register("PopupBorderThickness", typeof(Thickness), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness PopupBorderThickness
        {
            get
            {
                return (Thickness)GetValue(PopupBorderThicknessProperty);
            }
            set
            {
                SetValue(PopupBorderThicknessProperty, value);
            }
        }

        public static DependencyProperty PopupPlacementProperty = DependencyProperty.Register("PopupPlacement", typeof(PlacementMode), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(PlacementMode.Bottom, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public PlacementMode PopupPlacement
        {
            get
            {
                return (PlacementMode)GetValue(PopupPlacementProperty);
            }
            set
            {
                SetValue(PopupPlacementProperty, value);
            }
        }

        public static DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(CalendarSelectionMode), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(CalendarSelectionMode.SingleDate, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public CalendarSelectionMode SelectionMode
        {
            get
            {
                return (CalendarSelectionMode)GetValue(SelectionModeProperty);
            }
            set
            {
                SetValue(SelectionModeProperty, value);
            }
        }

        #endregion

        #region DateTimeUpDown

        public DateTimeUpDown() : base()
        {
            this.DefaultStyleKey = typeof(DateTimeUpDown);

            this.Increment = TimeSpan.FromDays(1.0);
            this.Minimum = DateTime.MinValue;
            this.Maximum = DateTime.MaxValue;
            this.Value = DateTime.Now;
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_Calendar = this.Template.FindName("PART_Calendar", this).As<Calendar>();
            this.PART_Calendar.SelectedDatesChanged += OnCalendarSelectedDatesChanged;

            this.PART_Popup = this.Template.FindName("PART_Popup", this).As<Popup>();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            if (!this.IsPopupOpen) this.IsPopupOpen = true;
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
            if (this.IsPopupOpen) this.IsPopupOpen = false;
        }

        #region Handlers

        void OnCalendarSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
                this.SetText(e.AddedItems[0].As<DateTime>());
        }

        #endregion

        #endregion
    }
}
