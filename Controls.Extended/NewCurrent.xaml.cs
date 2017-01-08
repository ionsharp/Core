using Imagin.Common.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public partial class NewCurrent : UserControl
    {
        #region Properties

        public static DependencyProperty NewColorProperty = DependencyProperty.Register("NewColor", typeof(Color), typeof(NewCurrent), new FrameworkPropertyMetadata(Colors.Gray, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty CurrentColorProperty = DependencyProperty.Register("CurrentColor", typeof(Color), typeof(NewCurrent), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// The color being selected 
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

        public static DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(NewCurrent), new PropertyMetadata((byte)255, new PropertyChangedCallback(OnAlphaChanged)));
        /// <summary>
        /// The Alpha Component of the currrent color
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
            var NewCurrent = (NewCurrent)d;
            NewCurrent.NewColor = NewCurrent.NewColor.WithAlpha(Convert.ToByte(e.NewValue));
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(NewCurrent), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure, OnOrientationChanged));
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var Orientation = (Orientation)e.NewValue;
            if (Orientation != (Orientation)e.OldValue)
            {
                var NewCurrent = (NewCurrent)d;
                if (Orientation == Orientation.Horizontal)
                    NewCurrent.SetOrientationToHorizontal();
                else NewCurrent.SetOrientationToVertical();
            }
        }

        #endregion

        #region NewCurrent

        public NewCurrent()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        void SetOrientationToHorizontal()
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

        void SetOrientationToVertical()
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

        #endregion
    }
}
