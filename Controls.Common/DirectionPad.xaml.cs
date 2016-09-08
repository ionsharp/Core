using Imagin.Common;
using Imagin.Common.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        #region Properties

        #region Private

        int[,] Directions = new int[3, 3]
        {
            { 0, 1, 2 },
            { 3, 4, 5 },
            { 6, 7, 8 }
        };

        int[,] Positions = new int[9, 2]
        {
            { 0, 0 },
            { 0, 1 },
            { 0, 2 },
            { 1, 0 },
            { 1, 1 },
            { 1, 2 },
            { 2, 0 },
            { 2, 1 },
            { 2, 2 }
        };

        ShiftType[] Shifts = new ShiftType[9]
        {
            ShiftType.Up | ShiftType.Left,
            ShiftType.Up,
            ShiftType.Up | ShiftType.Right,
            ShiftType.Left,
            ShiftType.None,
            ShiftType.Right,
            ShiftType.Down | ShiftType.Left,
            ShiftType.Down,
            ShiftType.Down | ShiftType.Right
        };

        #endregion

        #region Dependency

        internal static DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<Model>), typeof(DirectionPad), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        internal ObservableCollection<Model> Items
        {
            get
            {
                return (ObservableCollection<Model>)GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        public static DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(CompassDirection), typeof(DirectionPad), new FrameworkPropertyMetadata(CompassDirection.Origin, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCompassDirectionChanged));
        public CompassDirection Direction
        {
            get
            {
                return (CompassDirection)GetValue(DirectionProperty);
            }
            set
            {
                SetValue(DirectionProperty, value);
            }
        }
        private static void OnCompassDirectionChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            DirectionPad Pad = Object as DirectionPad;
            Pad.SetPositions(Pad.Direction);
        }

        #endregion

        #endregion

        #region Types

        [Flags]
        enum ShiftType
        {
            None = 0,
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }

        #endregion

        #region Model

       internal class Model : NamedObject
        {
            CompassDirection direction = CompassDirection.Unknown;
            public CompassDirection Direction
            {
                get
                {
                    return direction;
                }
                set
                {
                    this.direction = value;
                    OnPropertyChanged("Direction");
                }
            }

            string icon = string.Empty;
            public string Icon
            {
                get
                {
                    return icon;
                }
                set
                {
                    this.icon = value;
                    OnPropertyChanged("Icon");
                }
            }

            int row = 0;
            public int Row
            {
                get
                {
                    return row;
                }
                set
                {
                    this.row = value;
                    OnPropertyChanged("Row");
                }
            }

            int column = 0;
            public int Column
            {
                get
                {
                    return column;
                }
                set
                {
                    this.column = value;
                    OnPropertyChanged("Column");
                }
            }

            public int DefaultRow
            {
                get; private set;
            }

            public int DefaultColumn
            {
                get; private set;
            }

            public Model(string Name, CompassDirection Direction, string Icon, RowColumn RowColumn) : base(Name)
            {
                this.Direction = Direction;
                this.Icon = string.Concat(@"pack://application:,,,/Imagin.Controls.Common;component/Images/", Icon, ".png");
                this.Row = RowColumn.Row;
                this.Column = RowColumn.Column;
                this.DefaultRow = RowColumn.Row;
                this.DefaultColumn = RowColumn.Column;
            }
        }

        #endregion

        #region DirectionPad

        public DirectionPad()
        {
            InitializeComponent();

            this.Items = new ObservableCollection<Model>();
            this.Items.Add(new Model("Top Left", CompassDirection.NW, "ArrowNW", new RowColumn(1, 1)));
            this.Items.Add(new Model("Top", CompassDirection.N, "ArrowN", new RowColumn(1, 2)));
            this.Items.Add(new Model("Top Right", CompassDirection.NE, "ArrowNE", new RowColumn(1, 3)));
            this.Items.Add(new Model("Left", CompassDirection.W, "ArrowW", new RowColumn(2, 1)));
            this.Items.Add(new Model("Center", CompassDirection.Origin, "ArrowP", new RowColumn(2, 2)));
            this.Items.Add(new Model("Right", CompassDirection.E, "ArrowE", new RowColumn(2, 3)));
            this.Items.Add(new Model("Bottom Left", CompassDirection.SW, "ArrowSW", new RowColumn(3, 1)));
            this.Items.Add(new Model("Bottom", CompassDirection.S, "ArrowS", new RowColumn(3, 2)));
            this.Items.Add(new Model("Bottom Right", CompassDirection.SE, "ArrowSE", new RowColumn(3, 3)));
        }

        #endregion

        #region Methods

        void SetPositions(CompassDirection Direction)
        {
            int StartRow = this.Positions[(int)Direction, 0], StartColumn = this.Positions[(int)Direction, 1];
            int i = StartRow, j = StartColumn;
            foreach (Model d in this.Items)
            {
                if (j < StartColumn + 3)
                {
                    d.Row = i;
                    d.Column = j++;
                    if (j == (StartColumn + 3))
                    {
                        j = StartColumn;
                        i++;
                    }
                }
            }
        }

        void ShiftPositions(Model Model)
        {
            if (Model.Direction == CompassDirection.Origin)
                this.Direction = CompassDirection.Origin;

            ShiftType Shift = this.Shifts[(int)Model.Direction];
            if (Shift == ShiftType.None)
                return;

            foreach (Model d in this.Items)
            {
                if (Shift.HasFlag(ShiftType.Up)) d.Row--;
                if (Shift.HasFlag(ShiftType.Down)) d.Row++;
                if (Shift.HasFlag(ShiftType.Left)) d.Column--;
                if (Shift.HasFlag(ShiftType.Right)) d.Column++;
            }

            Model Origin = this.Items.Where(x => x.Direction == CompassDirection.Origin).First();
            RowColumn RowColumn = new RowColumn(Origin.Row, Origin.Column);
            this.Direction = (CompassDirection)this.Directions[--RowColumn.Row, --RowColumn.Column]; 
        }

        void OnClick(object sender, RoutedEventArgs e)
        {
            this.ShiftPositions(sender.As<FrameworkElement>().DataContext.As<Model>());
        }

        #endregion
    }
}
