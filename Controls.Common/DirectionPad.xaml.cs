using Imagin.Common;
using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A rectangular control that enables specifying a 
    /// direction by clicking a directional arrow. Directional 
    /// arrows make all arrows shift in corresponding direction
   ///  when clicked.
    /// </summary>
    public partial class DirectionPad : UserControl
    {
        #region Dependency Properties

        public static DependencyProperty CompassDirectionProperty = DependencyProperty.Register("CompassDirection", typeof(CompassDirection), typeof(DirectionPad), new FrameworkPropertyMetadata(CompassDirection.Origin, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCompassDirectionChanged));
        public CompassDirection CompassDirection
        {
            get
            {
                return (CompassDirection)GetValue(CompassDirectionProperty);
            }
            set
            {
                SetValue(CompassDirectionProperty, value);
            }
        }
        private static void OnCompassDirectionChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            DirectionPad Pad = Object as DirectionPad;
            Pad.SetPositions(Pad.CompassDirection);
        }

        #endregion

        #region DirectionPad

        public DirectionPad()
        {
            InitializeComponent();

            foreach (MaskedButton i in this.PART_Grid.Children)
                i.Click += this.OnClick;
        }

        #endregion

        #region Methods

        CompassDirection GetDirection(int Row, int Column)
        {
            if (Row == 1)
            {
                if (Column == 1)
                    return CompassDirection.NW;
                else if (Column == 2)
                    return CompassDirection.N;
                else if (Column == 3)
                    return CompassDirection.NE;
            }
            if (Row == 2)
            {
                if (Column == 1)
                    return CompassDirection.W;
                else if (Column == 2)
                    return CompassDirection.Origin;
                else if (Column == 3)
                    return CompassDirection.E;
            }
            if (Row == 3)
            {
                if (Column == 1)
                    return CompassDirection.SW;
                else if (Column == 2)
                    return CompassDirection.S;
                else if (Column == 3)
                    return CompassDirection.SE;
            }
            return CompassDirection.Unknown;
        }

        void SetPositions(CompassDirection CompassDirection)
        {
            int StartRow = 0, StartColumn = 0;
            switch (CompassDirection)
            {
                case CompassDirection.NW:
                    StartRow = 0;
                    StartColumn = 0;
                    break;
                case CompassDirection.N:
                    StartRow = 0;
                    StartColumn = 1;
                    break;
                case CompassDirection.NE:
                    StartRow = 0;
                    StartColumn = 2;
                    break;
                case CompassDirection.W:
                    StartRow = 1;
                    StartColumn = 0;
                    break;
                case CompassDirection.Origin:
                    StartRow = 1;
                    StartColumn = 1;
                    break;
                case CompassDirection.E:
                    StartRow = 1;
                    StartColumn = 2;
                    break;
                case CompassDirection.SW:
                    StartRow = 2;
                    StartColumn = 0;
                    break;
                case CompassDirection.S:
                    StartRow = 2;
                    StartColumn = 1;
                    break;
                case CompassDirection.SE:
                    StartRow = 2;
                    StartColumn = 2;
                    break;
            }
            int i = StartRow, j = StartColumn;
            foreach (MaskedButton b in this.PART_Grid.Children)
            {
                if (j < StartColumn + 3)
                {
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j++);
                    if (j == (StartColumn + 3))
                    {
                        j = StartColumn;
                        i++;
                    }
                }
            }
        }

        void ShiftPositions(CompassDirection CompassDirection, int Row, int Column)
        {
            bool ShiftUp = false, ShiftDown = false, ShiftLeft = false, ShiftRight = false;
            switch (CompassDirection)
            {
                case CompassDirection.NW:
                    if (Column == 0 || Row == 0) return;
                    ShiftUp = ShiftLeft = true;
                    break;
                case CompassDirection.N:
                    if (Row == 0) return;
                    ShiftUp = true;
                    break;
                case CompassDirection.NE:
                    if (Column > 3 || Row == 0) return;
                    ShiftUp = ShiftRight = true;
                    break;
                case CompassDirection.W:
                    if (Column == 0) return;
                    ShiftLeft = true;
                    break;
                case CompassDirection.E:
                    if (Column > 3) return;
                    ShiftRight = true;
                    break;
                case CompassDirection.SW:
                    if (Column == 0 || Row > 3) return;
                    ShiftDown = ShiftLeft = true;
                    break;
                case CompassDirection.S:
                    if (Row > 3) return;
                    ShiftDown = true;
                    break;
                case CompassDirection.SE:
                    if (Column > 3 || Row > 3) return;
                    ShiftDown = ShiftRight = true;
                    break;
                case CompassDirection.Origin:
                    this.CompassDirection = CompassDirection.Origin;
                    return;
                default:
                    return;
            }
            foreach (MaskedButton i in this.PART_Grid.Children)
            {
                int r = Grid.GetRow(i), c = Grid.GetColumn(i);
                Grid.SetRow(i, ShiftUp ? r - 1 : (ShiftDown ? r + 1 : r));
                Grid.SetColumn(i, ShiftLeft ? c - 1 : (ShiftRight ? c + 1 : c));
            }
            this.CompassDirection = this.GetDirection(Grid.GetRow(this.PART_OriginButton), Grid.GetColumn(this.PART_OriginButton));
        }

        #region Events

        void OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement Element = sender as FrameworkElement;
            this.ShiftPositions(Element.Tag.ToString().ParseEnum<CompassDirection>(), Grid.GetRow(Element), Grid.GetColumn(Element));
        }

        #endregion

        #endregion
    }
}
