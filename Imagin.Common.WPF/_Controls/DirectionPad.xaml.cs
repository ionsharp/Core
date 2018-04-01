using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common
{
    /// <summary>
    /// A rectangular <see cref="System.Windows.Controls.UserControl"/> that enables specifying a <see cref="CardinalDirection"/> relative to any given <see cref="CardinalDirection"/>. 
    /// </summary>
    public partial class DirectionPad : System.Windows.Controls.UserControl
    {
        #region Classes

        internal class ButtonViewModel : NamedObject
        {
            readonly public int DefaultColumn;

            readonly public int DefaultRow;

            readonly CardinalDirection _direction = CardinalDirection.Unknown;
            public CardinalDirection Direction => _direction;

            int _column = 0;
            public int Column
            {
                get => _column;
                set => Property.Set(this, ref _column, value);
            }

            string _icon = string.Empty;
            public string Icon
            {
                get => _icon;
                set => Property.Set(this, ref _icon, value);
            }

            int _row = 0;
            public int Row
            {
                get => _row;
                set => Property.Set(this, ref _row, value);
            }

            public ButtonViewModel(int column, int row, int direction) : base()
            {
                DefaultColumn = Column = column;
                DefaultRow = Row = row;
                _direction = (CardinalDirection)direction;
            }
        }

        #endregion

        #region Properties

        ObservableCollection<ButtonViewModel> _directions = new ObservableCollection<ButtonViewModel>()
        {
            new ButtonViewModel(1, 1, 0),
            new ButtonViewModel(2, 1, 1),
            new ButtonViewModel(3, 1, 2),
            new ButtonViewModel(1, 2, 3),
            new ButtonViewModel(2, 2, 4),
            new ButtonViewModel(3, 2, 5),
            new ButtonViewModel(1, 3, 6),
            new ButtonViewModel(2, 3, 7),
            new ButtonViewModel(3, 3, 8)
        };

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(CardinalDirection), typeof(DirectionPad), new FrameworkPropertyMetadata(CardinalDirection.Origin, OnDirectionChanged));
        /// <summary>
        /// 
        /// </summary>
        public CardinalDirection Direction
        {
            get
            {
                return (CardinalDirection)GetValue(DirectionProperty);
            }
            set
            {
                SetValue(DirectionProperty, value);
            }
        }
        static void OnDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DirectionPad).OnDirectionChanged((CardinalDirection)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ELabelProperty = DependencyProperty.Register("ELabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
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
        public static DependencyProperty EIconProperty = DependencyProperty.Register("EIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
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
        public static DependencyProperty NLabelProperty = DependencyProperty.Register("NLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
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
        public static DependencyProperty NIconProperty = DependencyProperty.Register("NIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
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
        public static DependencyProperty NELabelProperty = DependencyProperty.Register("NELabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
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
        public static DependencyProperty NEIconProperty = DependencyProperty.Register("NEIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
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
        public static DependencyProperty NWLabelProperty = DependencyProperty.Register("NWLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
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
        public static DependencyProperty NWIconProperty = DependencyProperty.Register("NWIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
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
        public static DependencyProperty OriginLabelProperty = DependencyProperty.Register("OriginLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
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
        public static DependencyProperty OriginIconProperty = DependencyProperty.Register("OriginIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
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
        public static DependencyProperty SLabelProperty = DependencyProperty.Register("SLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
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
        public static DependencyProperty SIconProperty = DependencyProperty.Register("SIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
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
        public static DependencyProperty SELabelProperty = DependencyProperty.Register("SELabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
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
        public static DependencyProperty SEIconProperty = DependencyProperty.Register("SEIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
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
        public static DependencyProperty SWLabelProperty = DependencyProperty.Register("SWLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
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
        public static DependencyProperty SWIconProperty = DependencyProperty.Register("SWIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
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
        public static DependencyProperty WLabelProperty = DependencyProperty.Register("WLabel", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
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
        public static DependencyProperty WIconProperty = DependencyProperty.Register("WIcon", typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
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

        static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DirectionPad).OnLabelChanged();

        static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DirectionPad).OnIconChanged();

        #endregion

        #region DirectionPad

        /// <summary>
        /// Initializes an instance of <see cref="DirectionPad"/>.
        /// </summary>
        public DirectionPad()
        {
            InitializeComponent();
            PART_Items.ItemsSource = _directions;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shift all relative to the specified <see cref="CardinalDirection"/>.
        /// </summary>
        /// <param name="direction"></param>
        void Shift(CardinalDirection direction)
        {
            var _direction = (int)direction;

            var shift = new int[,]
            {
                {-1, -1},
                {-1,  0},
                {-1,  1},
                { 0, -1},
                { 0,  0},
                { 0,  1},
                { 1, -1},
                { 1,  0},
                { 1,  1}
            };

            int x = shift[_direction, 1], y = shift[_direction, 0];

            if (x != 0 || y != 0)
            {
                foreach (var d in _directions)
                {
                    d.Column += x;
                    d.Row += y;
                }
            }
            else OnDirectionChanged(CardinalDirection.Origin);
        }

        /// <summary>
        /// Set <see cref="Direction"/> relative to the specified <see cref="ButtonViewModel"/>.
        /// </summary>
        /// <param name="button"></param>
        void Set(ButtonViewModel button)
        {
            var origin = button.Direction == CardinalDirection.Origin ? button : _directions.First(x => x.Direction == CardinalDirection.Origin);
            var result = (CardinalDirection)new int[,]
            {
                { 0, 1, 2 },
                { 3, 4, 5 },
                { 6, 7, 8 }
            }
            [origin.Row - 1, origin.Column - 1];
            SetCurrentValue(DirectionProperty, result);
        }

        /// <summary>
        /// Occurs when <see cref="Direction"/> changes.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnDirectionChanged(CardinalDirection value)
        {
            var positions = new int[,]
            {
                {0, 0},
                {0, 1},
                {0, 2},
                {1, 0},
                {1, 1},
                {1, 2},
                {2, 0},
                {2, 1},
                {2, 2}
            };

            int srow = positions[(int)value, 0],
                scolumn = positions[(int)value, 1];

            int y = srow, x = scolumn;

            foreach (var d in _directions)
            {
                if (x < scolumn + 3)
                {
                    d.Row = y;
                    d.Column = x++;
                    if (x == (scolumn + 3))
                    {
                        x = scolumn;
                        y++;
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when <see cref="NWLabel"/>, <see cref="NLabel"/>, <see cref="NELabel"/>, <see cref="WLabel"/>, <see cref="OriginLabel"/>, <see cref="ELabel"/>, <see cref="SWLabel"/>, <see cref="SLabel"/>, or <see cref="SELabel"/> changes.
        /// </summary>
        protected virtual void OnLabelChanged()
        {
            foreach (var i in _directions)
            {
                switch (i.Direction)
                {
                    case CardinalDirection.E:
                        i.Name = ELabel;
                        break;
                    case CardinalDirection.N:
                        i.Name = NLabel;
                        break;
                    case CardinalDirection.NE:
                        i.Name = NELabel;
                        break;
                    case CardinalDirection.NW:
                        i.Name = NWLabel;
                        break;
                    case CardinalDirection.Origin:
                        i.Name = OriginLabel;
                        break;
                    case CardinalDirection.S:
                        i.Name = SLabel;
                        break;
                    case CardinalDirection.SE:
                        i.Name = SELabel;
                        break;
                    case CardinalDirection.SW:
                        i.Name = SWLabel;
                        break;
                    case CardinalDirection.W:
                        i.Name = WLabel;
                        break;
                }
            }
        }

        /// <summary>
        /// Occurs when <see cref="NWIcon"/>, <see cref="NIcon"/>, <see cref="NEIcon"/>, <see cref="WIcon"/>, <see cref="OriginIcon"/>, <see cref="EIcon"/>, <see cref="SWIcon"/>, <see cref="SIcon"/>, or <see cref="SEIcon"/> changes.
        /// </summary>
        protected virtual void OnIconChanged()
        {
            foreach (var i in _directions)
            {
                switch (i.Direction)
                {
                    case CardinalDirection.E:
                        i.Icon = EIcon;
                        break;
                    case CardinalDirection.N:
                        i.Icon = NIcon;
                        break;
                    case CardinalDirection.NE:
                        i.Icon = NEIcon;
                        break;
                    case CardinalDirection.NW:
                        i.Icon = NWIcon;
                        break;
                    case CardinalDirection.Origin:
                        i.Icon = OriginIcon;
                        break;
                    case CardinalDirection.S:
                        i.Icon = SIcon;
                        break;
                    case CardinalDirection.SE:
                        i.Icon = SEIcon;
                        break;
                    case CardinalDirection.SW:
                        i.Icon = SWIcon;
                        break;
                    case CardinalDirection.W:
                        i.Icon = WIcon;
                        break;
                }
            }
        }

        ICommand shiftCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand ShiftCommand
        {
            get
            {
                shiftCommand = shiftCommand ?? new RelayCommand<object>(p =>
                {
                    var button = (ButtonViewModel)p;
                    Shift(button.Direction);
                      Set(button);
                },
                p => p is ButtonViewModel);
                return shiftCommand;
            }
        }

        #endregion
    }
}