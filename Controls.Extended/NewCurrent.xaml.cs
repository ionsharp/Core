using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NewCurrent : UserControl
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NewColorProperty = DependencyProperty.Register("NewColor", typeof(Color), typeof(NewCurrent), new FrameworkPropertyMetadata(Colors.Gray, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Color NewColor
        {
            get
            {
                return (Color)GetValue(NewColorProperty);
            }
            set
            {
                SetValue(NewColorProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CurrentColorProperty = DependencyProperty.Register("CurrentColor", typeof(Color), typeof(NewCurrent), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Color CurrentColor
        {
            get
            {
                return (Color)GetValue(CurrentColorProperty);
            }
            set
            {
                SetValue(CurrentColorProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(NewCurrent), new PropertyMetadata((byte)255, new PropertyChangedCallback(OnAlphaChanged)));
        /// <summary>
        /// 
        /// </summary>
        public byte Alpha
        {
            get
            {
                return (byte)GetValue(AlphaProperty);
            }
            set
            {
                SetValue(AlphaProperty, value);
            }
        }
        static void OnAlphaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<NewCurrent>().OnAlphaChanged((byte)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(NewCurrent), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure, OnOrientationChanged));
        /// <summary>
        /// 
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<NewCurrent>().OnOrientationChanged((Orientation)e.OldValue, (Orientation)e.NewValue);
        }

        #endregion

        #region NewCurrent

        /// <summary>
        /// 
        /// </summary>

        public NewCurrent()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        void SetHorizontal()
        {
            var star = new GridLength(50, GridUnitType.Star);
            var auto = GridLength.Auto;

            LayoutRoot.ColumnDefinitions[0].Width = star;
            LayoutRoot.ColumnDefinitions[1].Width = star;

            LayoutRoot.RowDefinitions[0].Height = auto;
            LayoutRoot.RowDefinitions[1].Height = star;
            LayoutRoot.RowDefinitions[2].Height = auto;
            LayoutRoot.RowDefinitions[3].Height = auto;

            PART_CurrentLabel.SetValue(Grid.RowProperty, 0);
            PART_CurrentLabel.SetValue(Grid.ColumnProperty, 0);

            PART_NewLabel.SetValue(Grid.RowProperty, 0);
            PART_NewLabel.SetValue(Grid.ColumnProperty, 1);

            PART_NewRectangle.SetValue(Grid.RowProperty, 1);
            PART_NewRectangle.SetValue(Grid.ColumnProperty, 1);
            PART_NewCheckered.SetValue(Grid.RowProperty, 1);
            PART_NewCheckered.SetValue(Grid.ColumnProperty, 1);

            PART_CurrentRectangle.SetValue(Grid.RowProperty, 1);
            PART_CurrentRectangle.SetValue(Grid.ColumnProperty, 0);
            PART_CurrentCheckered.SetValue(Grid.RowProperty, 1);
            PART_CurrentCheckered.SetValue(Grid.ColumnProperty, 0);
        }

        void SetVertical()
        {
            var star = new GridLength(35, GridUnitType.Star);
            var auto = GridLength.Auto;

            LayoutRoot.ColumnDefinitions[0].Width = star;
            LayoutRoot.ColumnDefinitions[1].Width = auto;

            LayoutRoot.RowDefinitions[0].Height = auto;
            LayoutRoot.RowDefinitions[1].Height = star;
            LayoutRoot.RowDefinitions[2].Height = star;
            LayoutRoot.RowDefinitions[3].Height = auto;

            PART_NewLabel.SetValue(Grid.RowProperty, 0);
            PART_NewLabel.SetValue(Grid.ColumnProperty, 0);

            PART_NewRectangle.SetValue(Grid.RowProperty, 1);
            PART_NewRectangle.SetValue(Grid.ColumnProperty, 0);
            PART_NewCheckered.SetValue(Grid.RowProperty, 1);
            PART_NewCheckered.SetValue(Grid.ColumnProperty, 0);

            PART_CurrentRectangle.SetValue(Grid.RowProperty, 2);
            PART_CurrentRectangle.SetValue(Grid.ColumnProperty, 0);
            PART_CurrentCheckered.SetValue(Grid.RowProperty, 2);
            PART_CurrentCheckered.SetValue(Grid.ColumnProperty, 0);

            PART_CurrentLabel.SetValue(Grid.RowProperty, 3);
            PART_CurrentLabel.SetValue(Grid.ColumnProperty, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnAlphaChanged(byte Value)
        {
            NewColor = NewColor.WithAlpha(Convert.ToByte(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnOrientationChanged(Orientation OldValue, Orientation NewValue)
        {
            if (OldValue != NewValue)
            {
                if (NewValue == Orientation.Horizontal)
                {
                    SetHorizontal();
                }
                else SetVertical();
            }
        }

        #endregion
    }
}
