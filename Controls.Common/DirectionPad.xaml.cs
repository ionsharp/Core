using Imagin.Common;
using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
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
        }

        #endregion

        #region Methods

        void SetCompassDirection()
        {
            int Row = Grid.GetRow(this.PART_OriginButton);
            int Column = Grid.GetColumn(this.PART_OriginButton);
            switch (Row)
            {
                case 1:
                    if (Column == 1)
                        this.CompassDirection = CompassDirection.NW;
                    else if (Column == 2)
                        this.CompassDirection = CompassDirection.N;
                    else if (Column == 3)
                        this.CompassDirection = CompassDirection.NE;
                    break;
                case 2:
                    if (Column == 1)
                        this.CompassDirection = CompassDirection.W;
                    else if (Column == 2)
                        this.CompassDirection = CompassDirection.Origin;
                    else if (Column == 3)
                        this.CompassDirection = CompassDirection.E;
                    break;
                case 3:
                    if (Column == 1)
                        this.CompassDirection = CompassDirection.SW;
                    else if (Column == 2)
                        this.CompassDirection = CompassDirection.S;
                    else if (Column == 3)
                        this.CompassDirection = CompassDirection.SE;
                    break;
            }
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
                    //CompassDirection.Origin
            }
            int i = StartRow, j = StartColumn;
            foreach (UIElement CurrentButton in this.Grid.Children)
            {
                if (!(CurrentButton is Button)) continue;
                if (j < StartColumn + 3)
                {
                    Grid.SetRow(CurrentButton, i);
                    Grid.SetColumn(CurrentButton, j++);
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
                    if (Column - 1 < 0 || Row - 1 < 0) return;
                    ShiftUp = ShiftLeft = true;
                    break;
                case CompassDirection.N:
                    if (Row - 1 < 0) return;
                    ShiftUp = true;
                    break;
                case CompassDirection.NE:
                    if (Column + 1 > 4 || Row - 1 < 0) return;
                    ShiftUp = ShiftRight = true;
                    break;
                case CompassDirection.W:
                    if (Column - 1 < 0) return;
                    ShiftLeft = true;
                    break;
                case CompassDirection.E:
                    if (Column + 1 > 4) return;
                    ShiftRight = true;
                    break;
                case CompassDirection.SW:
                    if (Column - 1 < 0 || Row + 1 > 4) return;
                    ShiftDown = ShiftLeft = true;
                    break;
                case CompassDirection.S:
                    if (Row + 1 > 4) return;
                    ShiftDown = true;
                    break;
                case CompassDirection.SE:
                    if (Column + 1 > 4 || Row + 1 > 4) return;
                    ShiftDown = ShiftRight = true;
                    break;
                case CompassDirection.Origin:
                    this.CompassDirection = CompassDirection.Origin;
                    return;
                default:
                    return;
            }
            foreach (UIElement CurrentButton in this.Grid.Children)
            {
                if (CurrentButton is Button)
                {
                    int CurrentRow = Grid.GetRow(CurrentButton), CurrentColumn = Grid.GetColumn(CurrentButton);
                    Grid.SetRow(CurrentButton, ShiftUp ? CurrentRow - 1 : (ShiftDown ? CurrentRow + 1 : CurrentRow));
                    Grid.SetColumn(CurrentButton, ShiftLeft ? CurrentColumn - 1 : (ShiftRight ? CurrentColumn + 1 : CurrentColumn));
                }
            }
            this.SetCompassDirection();
        }

        #region Events

        void OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement Element = sender as FrameworkElement;
            string Tag = Element.Tag.ToString();

            int Row = Grid.GetRow(Element);
            int Column = Grid.GetColumn(Element);

            this.ShiftPositions(Tag.ParseEnum<CompassDirection>(), Row, Column);
        }

        #endregion

        #endregion
    }
}
