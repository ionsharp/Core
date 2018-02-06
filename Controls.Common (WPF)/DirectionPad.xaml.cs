using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Primitives;
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
    public partial class DirectionPad : System.Windows.Controls.UserControl
    {
        #region Properties

        #region Private

        int[,] directions = new int[3, 3]
        {
            { 0, 1, 2 },
            { 3, 4, 5 },
            { 6, 7, 8 }
        };

        int[,] positions = new int[9, 2]
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

        ShiftType[] shifts = new ShiftType[9]
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(CompassPoint), typeof(DirectionPad), new FrameworkPropertyMetadata(CompassPoint.Origin, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCompassPointChanged));
        /// <summary>
        /// 
        /// </summary>
        public CompassPoint Direction
        {
            get
            {
                return (CompassPoint)GetValue(DirectionProperty);
            }
            set
            {
                SetValue(DirectionProperty, value);
            }
        }
        static void OnCompassPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DirectionPad).SetPositions((d as DirectionPad).Direction);
        }

        internal static DependencyProperty DirectionsProperty = DependencyProperty.Register("Directions", typeof(ObservableCollection<Model>), typeof(DirectionPad), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        internal ObservableCollection<Model> Directions
        {
            get
            {
                return (ObservableCollection<Model>)GetValue(DirectionsProperty);
            }
            set
            {
                SetValue(DirectionsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ELabelProperty = DependencyProperty.Register("ELabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLabelChanged));
        /// <summary>
        /// 
        /// </summary>
        public string ELabel
        {
            get
            {
                return (string)GetValue(ELabelProperty);
            }
            set
            {
                SetValue(ELabelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty EIconProperty = DependencyProperty.Register("EIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        /// <summary>
        /// 
        /// </summary>
        public string EIcon
        {
            get
            {
                return (string)GetValue(EIconProperty);
            }
            set
            {
                SetValue(EIconProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NLabelProperty = DependencyProperty.Register("NLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLabelChanged));
        /// <summary>
        /// 
        /// </summary>
        public string NLabel
        {
            get
            {
                return (string)GetValue(NLabelProperty);
            }
            set
            {
                SetValue(NLabelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NIconProperty = DependencyProperty.Register("NIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        /// <summary>
        /// 
        /// </summary>
        public string NIcon
        {
            get
            {
                return (string)GetValue(NIconProperty);
            }
            set
            {
                SetValue(NIconProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NELabelProperty = DependencyProperty.Register("NELabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLabelChanged));
        /// <summary>
        /// 
        /// </summary>
        public string NELabel
        {
            get
            {
                return (string)GetValue(NELabelProperty);
            }
            set
            {
                SetValue(NELabelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NEIconProperty = DependencyProperty.Register("NEIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        /// <summary>
        /// 
        /// </summary>
        public string NEIcon
        {
            get
            {
                return (string)GetValue(NEIconProperty);
            }
            set
            {
                SetValue(NEIconProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NWLabelProperty = DependencyProperty.Register("NWLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLabelChanged));
        /// <summary>
        /// 
        /// </summary>
        public string NWLabel
        {
            get
            {
                return (string)GetValue(NWLabelProperty);
            }
            set
            {
                SetValue(NWLabelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NWIconProperty = DependencyProperty.Register("NWIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        /// <summary>
        /// 
        /// </summary>
        public string NWIcon
        {
            get
            {
                return (string)GetValue(NWIconProperty);
            }
            set
            {
                SetValue(NWIconProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OriginLabelProperty = DependencyProperty.Register("OriginLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLabelChanged));
        /// <summary>
        /// 
        /// </summary>
        public string OriginLabel
        {
            get
            {
                return (string)GetValue(OriginLabelProperty);
            }
            set
            {
                SetValue(OriginLabelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OriginIconProperty = DependencyProperty.Register("OriginIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        /// <summary>
        /// 
        /// </summary>
        public string OriginIcon
        {
            get
            {
                return (string)GetValue(OriginIconProperty);
            }
            set
            {
                SetValue(OriginIconProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SLabelProperty = DependencyProperty.Register("SLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLabelChanged));
        /// <summary>
        /// 
        /// </summary>
        public string SLabel
        {
            get
            {
                return (string)GetValue(SLabelProperty);
            }
            set
            {
                SetValue(SLabelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SIconProperty = DependencyProperty.Register("SIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        /// <summary>
        /// 
        /// </summary>
        public string SIcon
        {
            get
            {
                return (string)GetValue(SIconProperty);
            }
            set
            {
                SetValue(SIconProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SELabelProperty = DependencyProperty.Register("SELabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLabelChanged));
        /// <summary>
        /// 
        /// </summary>
        public string SELabel
        {
            get
            {
                return (string)GetValue(SELabelProperty);
            }
            set
            {
                SetValue(SELabelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SEIconProperty = DependencyProperty.Register("SEIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        /// <summary>
        /// 
        /// </summary>
        public string SEIcon
        {
            get
            {
                return (string)GetValue(SEIconProperty);
            }
            set
            {
                SetValue(SEIconProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SWLabelProperty = DependencyProperty.Register("SWLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLabelChanged));
        /// <summary>
        /// 
        /// </summary>
        public string SWLabel
        {
            get
            {
                return (string)GetValue(SWLabelProperty);
            }
            set
            {
                SetValue(SWLabelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SWIconProperty = DependencyProperty.Register("SWIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        /// <summary>
        /// 
        /// </summary>
        public string SWIcon
        {
            get
            {
                return (string)GetValue(SWIconProperty);
            }
            set
            {
                SetValue(SWIconProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty WLabelProperty = DependencyProperty.Register("WLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLabelChanged));
        /// <summary>
        /// 
        /// </summary>
        public string WLabel
        {
            get
            {
                return (string)GetValue(WLabelProperty);
            }
            set
            {
                SetValue(WLabelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty WIconProperty = DependencyProperty.Register("WIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIconChanged));
        /// <summary>
        /// 
        /// </summary>
        public string WIcon
        {
            get
            {
                return (string)GetValue(WIconProperty);
            }
            set
            {
                SetValue(WIconProperty, value);
            }
        }

        static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DirectionPad).OnLabelChanged();
        }

        static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DirectionPad).OnIconChanged();
        }

        #endregion

        #endregion

        #region Types

        internal class Model : NamedObject
        {
            CompassPoint direction = CompassPoint.Unknown;
            public CompassPoint Direction
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

            public Model(CompassPoint direction, RowColumn rowColumn) : base(string.Empty)
            {
                Direction = direction;

                Row = rowColumn.Row.ToInt32();
                Column = rowColumn.Column.ToInt32();

                DefaultRow = rowColumn.Row.ToInt32();
                DefaultColumn = rowColumn.Column.ToInt32();
            }
        }

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

        #region DirectionPad

        /// <summary>
        /// 
        /// </summary>
        public DirectionPad()
        {
            SetCurrentValue(DirectionsProperty, new ObservableCollection<Model>());
            Directions.Add(new Model(CompassPoint.NW, new RowColumn(1, 1)));
            Directions.Add(new Model(CompassPoint.N, new RowColumn(1, 2)));
            Directions.Add(new Model(CompassPoint.NE, new RowColumn(1, 3)));
            Directions.Add(new Model(CompassPoint.W, new RowColumn(2, 1)));
            Directions.Add(new Model(CompassPoint.Origin, new RowColumn(2, 2)));
            Directions.Add(new Model(CompassPoint.E, new RowColumn(2, 3)));
            Directions.Add(new Model(CompassPoint.SW, new RowColumn(3, 1)));
            Directions.Add(new Model(CompassPoint.S, new RowColumn(3, 2)));
            Directions.Add(new Model(CompassPoint.SE, new RowColumn(3, 3)));

            InitializeComponent();
        }

        #endregion

        #region Methods

        void SetPositions(CompassPoint Direction)
        {
            int StartRow = positions[(int)Direction, 0], StartColumn = positions[(int)Direction, 1];
            int i = StartRow, j = StartColumn;
            foreach (var d in Directions)
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
            if (Model.Direction == CompassPoint.Origin)
                Direction = CompassPoint.Origin;

            var Shift = shifts[(int)Model.Direction];
            if (Shift == ShiftType.None) return;

            foreach (var d in Directions)
            {
                if (Shift.HasFlag(ShiftType.Up)) d.Row--;
                if (Shift.HasFlag(ShiftType.Down)) d.Row++;
                if (Shift.HasFlag(ShiftType.Left)) d.Column--;
                if (Shift.HasFlag(ShiftType.Right)) d.Column++;
            }

            var Origin = Directions.Where(x => x.Direction == CompassPoint.Origin).First();
            var RowColumn = new RowColumn(Origin.Row, Origin.Column);
            Direction = (CompassPoint)directions[(--RowColumn.Row).ToInt32(), (--RowColumn.Column).ToInt32()]; 
        }

        void OnShifted(object sender, RoutedEventArgs e)
        {
            ShiftPositions(sender.As<FrameworkElement>().DataContext.As<Model>());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnLabelChanged()
        {
            foreach (var i in Directions)
            {
                switch (i.Direction)
                {
                    case CompassPoint.E:
                        i.Name = ELabel;
                        break;
                    case CompassPoint.N:
                        i.Name = NLabel;
                        break;
                    case CompassPoint.NE:
                        i.Name = NELabel;
                        break;
                    case CompassPoint.NW:
                        i.Name = NWLabel;
                        break;
                    case CompassPoint.Origin:
                        i.Name = OriginLabel;
                        break;
                    case CompassPoint.S:
                        i.Name = SLabel;
                        break;
                    case CompassPoint.SE:
                        i.Name = SELabel;
                        break;
                    case CompassPoint.SW:
                        i.Name = SWLabel;
                        break;
                    case CompassPoint.W:
                        i.Name = WLabel;
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnIconChanged()
        {
            foreach (var i in Directions)
            {
                switch (i.Direction)
                {
                    case CompassPoint.E:
                        i.Icon = EIcon;
                        break;
                    case CompassPoint.N:
                        i.Icon = NIcon;
                        break;
                    case CompassPoint.NE:
                        i.Icon = NEIcon;
                        break;
                    case CompassPoint.NW:
                        i.Icon = NWIcon;
                        break;
                    case CompassPoint.Origin:
                        i.Icon = OriginIcon;
                        break;
                    case CompassPoint.S:
                        i.Icon = SIcon;
                        break;
                    case CompassPoint.SE:
                        i.Icon = SEIcon;
                        break;
                    case CompassPoint.SW:
                        i.Icon = SWIcon;
                        break;
                    case CompassPoint.W:
                        i.Icon = WIcon;
                        break;
                }
            }
        }

        #endregion
    }
}
